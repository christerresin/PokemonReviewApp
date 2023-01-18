﻿using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repositories
{
    public class ClubRespository : IClubRepository
    {
        private readonly ApplicationDbContext _context;
        public ClubRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Club club)
        {
            // Calling Add in EF core does NOTE add and save the entity to the DB, it only generates "tracked/scoped" a SQL query
            _context.Add(club);
            // Calling Save() runs all the tracked in scope SQL querys
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c => c.Address!.City.Contains(city)).ToListAsync();
        }

        public async Task<Club> GetClubById(int id)
        {
            return await _context.Clubs.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Club> GetClubByIdNoTracking(int id)
        {
            return await _context.Clubs.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save(); 
        }
    }
}
