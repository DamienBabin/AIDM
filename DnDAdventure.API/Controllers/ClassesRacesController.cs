using Microsoft.AspNetCore.Mvc;
using DnDAdventure.Core.Models;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesRacesController : ControllerBase
    {
        private readonly DndClassesAndRaces _classesAndRaces;

        public ClassesRacesController()
        {
            _classesAndRaces = new DndClassesAndRaces();
        }

        [HttpGet("classes")]
        public ActionResult<List<DndClass>> GetClasses()
        {
            return Ok(_classesAndRaces.Classes);
        }

        [HttpGet("races")]
        public ActionResult<List<DndRace>> GetRaces()
        {
            return Ok(_classesAndRaces.Races);
        }

        [HttpGet("class/{name}")]
        public ActionResult<DndClass> GetClassByName(string name)
        {
            var dndClass = _classesAndRaces.Classes.FirstOrDefault(c => 
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
            var race = _classesAndRaces.Races.FirstOrDefault(r => 
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
            var race = _classesAndRaces.Races.FirstOrDefault(r => 
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
            var dndClass = _classesAndRaces.Classes.FirstOrDefault(c => 
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
            var race = _classesAndRaces.Races.FirstOrDefault(r => 
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
            var dndClass = _classesAndRaces.Classes.FirstOrDefault(c => 
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
