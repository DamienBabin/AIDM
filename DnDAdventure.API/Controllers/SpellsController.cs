using Microsoft.AspNetCore.Mvc;
using DnDAdventure.Core.Models;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpellsController : ControllerBase
    {
        private readonly SpellList _spellList;

        public SpellsController()
        {
            _spellList = new SpellList();
        }

        [HttpGet("cantrips")]
        public ActionResult<IEnumerable<Spell>> GetCantrips()
        {
            var cantrips = _spellList.Spells.Where(s => s.Level == 0).ToList();
            return Ok(cantrips);
        }

        [HttpGet("cantrips/{className}")]
        public ActionResult<IEnumerable<Spell>> GetCantripsByClass(string className)
        {
            var cantrips = _spellList.Spells
                .Where(s => s.Level == 0 && s.Classes.Contains(className))
                .ToList();
            return Ok(cantrips);
        }

        [HttpGet("spells")]
        public ActionResult<IEnumerable<Spell>> GetSpells()
        {
            return Ok(_spellList.Spells);
        }

        [HttpGet("spells/{className}")]
        public ActionResult<IEnumerable<Spell>> GetSpellsByClass(string className)
        {
            var spells = _spellList.Spells
                .Where(s => s.Classes.Contains(className))
                .ToList();
            return Ok(spells);
        }

        [HttpGet("spells/{className}/level/{level}")]
        public ActionResult<IEnumerable<Spell>> GetSpellsByClassAndLevel(string className, int level)
        {
            var spells = _spellList.Spells
                .Where(s => s.Level == level && s.Classes.Contains(className))
                .ToList();
            return Ok(spells);
        }

        [HttpGet("{spellName}")]
        public ActionResult<Spell> GetSpell(string spellName)
        {
            var spell = _spellList.Spells.FirstOrDefault(s => 
                s.Name.Equals(spellName, StringComparison.OrdinalIgnoreCase));
            
            if (spell == null)
            {
                return NotFound($"Spell '{spellName}' not found");
            }

            return Ok(spell);
        }
    }
}
