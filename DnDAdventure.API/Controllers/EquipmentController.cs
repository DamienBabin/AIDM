using Microsoft.AspNetCore.Mvc;
using DnDAdventure.Core.Models;

namespace DnDAdventure.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly EquipmentList _equipmentList;

        public EquipmentController()
        {
            _equipmentList = new EquipmentList();
        }

        [HttpGet("weapons")]
        public ActionResult<List<Weapon>> GetWeapons()
        {
            return Ok(_equipmentList.Weapons);
        }

        [HttpGet("armor")]
        public ActionResult<List<Armor>> GetArmor()
        {
            return Ok(_equipmentList.Armors);
        }

        [HttpGet("weapons/{type}")]
        public ActionResult<List<Weapon>> GetWeaponsByType(WeaponType type)
        {
            return Ok(_equipmentList.GetWeaponsByType(type));
        }

        [HttpGet("armor/{type}")]
        public ActionResult<List<Armor>> GetArmorByType(ArmorType type)
        {
            return Ok(_equipmentList.GetArmorsByType(type));
        }

        [HttpGet("weapon/{name}")]
        public ActionResult<Weapon> GetWeaponByName(string name)
        {
            var weapon = _equipmentList.GetWeaponByName(name);
            if (weapon == null)
            {
                return NotFound();
            }
            return Ok(weapon);
        }

        [HttpGet("armor/{name}")]
        public ActionResult<Armor> GetArmorByName(string name)
        {
            var armor = _equipmentList.GetArmorByName(name);
            if (armor == null)
            {
                return NotFound();
            }
            return Ok(armor);
        }
    }
}
