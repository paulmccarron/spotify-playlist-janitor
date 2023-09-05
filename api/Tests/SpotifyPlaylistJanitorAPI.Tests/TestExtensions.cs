using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyPlaylistJanitorAPI.Tests
{
    public static class TestExtensions
    {
        public static void AddIQueryables<T>(this Mock<DbSet<T>> mockDbSet, IQueryable<T> queryables) where T : class
        {
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryables.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryables.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryables.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryables.GetEnumerator);
        }
    }
}
