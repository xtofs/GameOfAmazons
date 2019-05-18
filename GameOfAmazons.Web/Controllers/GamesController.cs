using System;
using GameOfAmazons.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameOfAmazons.Web.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {

        private static Game game = new Game(new int[6, 6] {
                    { 0, 0, 1, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 2 },
                    { 2, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 0, 0, 0 },
                    { 0, 0, 0, 1, 0, 0 }
                });
            
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
           

            return Ok(game);
        }

        [HttpGet("{id}/legalMoves")]
        public IActionResult GetLegalMoves(string id)
        {
            return Ok(game.LegalMoves());
        }
    }

}
