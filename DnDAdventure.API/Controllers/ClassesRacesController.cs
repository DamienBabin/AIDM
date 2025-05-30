using Microsoft.AspNetCore.Mvc;
using DnDAdventure.Core.Models.Classes;
using DnDAdventure.Core.Models.Races;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesRacesController : ControllerBase
    {
        private readonly List<DndClass> _classes;
        private readonly List<DndRace> _races;

        public ClassesRacesController()
        {
            _classes = ClassesData.GetClasses();
            _races = RacesData.GetRaces();
        }

        [HttpGet("races/flattened")]
        public ActionResult<List<object>> GetFlattenedRaces()
        {
            var flattenedRaces = new List<object>();
            
            foreach (var race in _races)
            {
                if (race.Subraces != null && race.Subraces.Count > 0)
                {
                    // Add each subrace as a separate option
                    foreach (var subrace in race.Subraces)
                    {
                        flattenedRaces.Add(new
                        {
                            Name = $"{race.Name} ({subrace.Name})",
                            DisplayName = $"{race.Name} ({subrace.Name})",
                            RaceName = race.Name,
                            SubraceName = subrace.Name,
                            IsSubrace = true,
                            Description = subrace.Description,
                            AbilityScoreIncrease = MergeAbilityScores(race.AbilityScoreIncrease, subrace.AbilityScoreIncrease),
                            Speed = race.Speed,
                            Languages = race.Languages,
                            Traits = race.Traits.Concat(subrace.Traits).ToList(),
                            Source = subrace.Source,
                            Pros = subrace.Pros,
                            Cons = subrace.Cons,
                            BestFor = subrace.BestFor,
                            PlayStyle = subrace.PlayStyle
                        });
                    }
                }
                else
                {
                    // Add race without subraces
                    flattenedRaces.Add(new
                    {
                        Name = race.Name,
                        DisplayName = race.Name,
                        RaceName = race.Name,
                        SubraceName = (string)null,
                        IsSubrace = false,
                        Description = race.Description,
                        AbilityScoreIncrease = race.AbilityScoreIncrease,
                        Speed = race.Speed,
                        Languages = race.Languages,
                        Traits = race.Traits,
                        Source = race.Source,
                        Pros = new List<string>(),
                        Cons = new List<string>(),
                        BestFor = new List<string>(),
                        PlayStyle = string.Empty
                    });
                }
            }
            
            return Ok(flattenedRaces);
        }

        private Dictionary<string, int> MergeAbilityScores(Dictionary<string, int> raceScores, Dictionary<string, int> subraceScores)
        {
            var merged = new Dictionary<string, int>(raceScores);
            
            foreach (var kvp in subraceScores)
            {
                if (merged.ContainsKey(kvp.Key))
                {
                    merged[kvp.Key] += kvp.Value;
                }
                else
                {
                    merged[kvp.Key] = kvp.Value;
                }
            }
            
            return merged;
        }

        [HttpGet("classes")]
        public ActionResult<List<DndClass>> GetClasses()
        {
            return Ok(_classes);
        }

        [HttpGet("races")]
        public ActionResult<List<DndRace>> GetRaces()
        {
            return Ok(_races);
        }

        [HttpGet("class/{name}")]
        public ActionResult<DndClass> GetClassByName(string name)
        {
            var dndClass = _classes.FirstOrDefault(c => 
                c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            
            if (dndClass == null)
            {
                return NotFound();
            }
            
            return Ok(dndClass);
        }

        [HttpGet("race/{name}")]
        public ActionResult<DndRace> GetRaceByName(string name)
        {
            var race = _races.FirstOrDefault(r => 
                r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            
            if (race == null)
            {
                return NotFound();
            }
            
            return Ok(race);
        }

        [HttpGet("race/{raceName}/subraces")]
        public ActionResult<List<string>> GetSubraces(string raceName)
        {
            var race = _races.FirstOrDefault(r => 
                r.Name.Equals(raceName, StringComparison.OrdinalIgnoreCase));
            
            if (race == null)
            {
                return NotFound();
            }
            
            return Ok(race.Subraces.Select(s => s.Name).ToList());
        }

        [HttpGet("class/{className}/subclasses")]
        public ActionResult<List<string>> GetSubclasses(string className)
        {
            var dndClass = _classes.FirstOrDefault(c => 
                c.Name.Equals(className, StringComparison.OrdinalIgnoreCase));
            
            if (dndClass == null)
            {
                return NotFound();
            }
            
            return Ok(dndClass.Subclasses.Select(s => s.Name).ToList());
        }

        [HttpGet("race/{raceName}/subrace/{subraceName}")]
        public ActionResult<Subrace> GetSubraceByName(string raceName, string subraceName)
        {
            var race = _races.FirstOrDefault(r => 
                r.Name.Equals(raceName, StringComparison.OrdinalIgnoreCase));
            
            if (race == null)
            {
                return NotFound("Race not found");
            }

            var subrace = race.Subraces.FirstOrDefault(s => 
                s.Name.Equals(subraceName, StringComparison.OrdinalIgnoreCase));
            
            if (subrace == null)
            {
                return NotFound("Subrace not found");
            }
            
            return Ok(subrace);
        }

        [HttpGet("class/{className}/subclass/{subclassName}")]
        public ActionResult<Subclass> GetSubclassByName(string className, string subclassName)
        {
            var dndClass = _classes.FirstOrDefault(c => 
                c.Name.Equals(className, StringComparison.OrdinalIgnoreCase));
            
            if (dndClass == null)
            {
                return NotFound("Class not found");
            }

            var subclass = dndClass.Subclasses.FirstOrDefault(s => 
                s.Name.Equals(subclassName, StringComparison.OrdinalIgnoreCase));
            
            if (subclass == null)
            {
                return NotFound("Subclass not found");
            }
            
            return Ok(subclass);
        }
    }
}
