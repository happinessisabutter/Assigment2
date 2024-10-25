using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Data;
using WinFormsApp1.Models;

namespace WinFormsApp1.Repository
{
    public class RoomRepository : IRepository<Room>
    {
        private readonly CinemaAppDbcontext _context;

        public RoomRepository(CinemaAppDbcontext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Room entity)
        {
            await _context.Rooms.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
        public async Task<bool> UpdateAsync(Room entity)
        {
            _context.Rooms.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var room = await GetByIdAsync(id);
            if (room == null)
            {
                return false;
            }
            _context.Rooms.Remove(room);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
