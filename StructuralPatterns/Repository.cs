using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StructuralPatterns
{
    public class Repository
    {
        #region Model

        public class Tag
        {
            public Tag()
            {
                Courses = new HashSet<Course>();
            }

            public int Id { get; set; }

            public string Name { get; set; }

            public virtual ICollection<Course> Courses { get; set; }
        }

        public class Cover
        {
            public int Id { get; set; }
            public Course Course { get; set; }
        }

        public class Course
        {
            public Course()
            {
                Tags = new HashSet<Tag>();
            }

            public int Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public int Level { get; set; }

            public float FullPrice { get; set; }

            public virtual Author Author { get; set; }

            public int AuthorId { get; set; }

            public virtual ICollection<Tag> Tags { get; set; }

            public Cover Cover { get; set; }

            public bool IsBeginnerCourse
            {
                get { return Level == 1; }
            }
        }

        public class Author
        {
            public Author()
            {
                Courses = new HashSet<Course>();
            }

            public int Id { get; set; }

            public string Name { get; set; }

            public virtual ICollection<Course> Courses { get; set; }
        }

        public class CustomContext : DbContext
        {
            public CustomContext() : base() { }

            public virtual DbSet<Author> Authors { get; set; }
            public virtual DbSet<Course> Courses { get; set; }
            public virtual DbSet<Tag> Tags { get; set; }
        }

        #endregion

        #region Generic Repository

        public interface IRepository<TEntity> where TEntity : class
        {
            TEntity Get(int id);
            IEnumerable<TEntity> GetAll();
            IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
            TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

            bool Exists(Expression<Func<TEntity, bool>> predicate);

            bool Add(TEntity entity);
            bool AddRange(IEnumerable<TEntity> entities);

            bool Update(TEntity entity);

            bool Remove(TEntity entity);
            bool RemoveRange(IEnumerable<TEntity> entities);

            int Save();
        }

        public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
        {
            protected readonly DbContext Context;

            public RepositoryBase(DbContext context)
            {
                Context = context;
            }

            public TEntity Get(int id)
            {
                return Context.Set<TEntity>().Find(id);
            }

            public IEnumerable<TEntity> GetAll()
            {
                return Context.Set<TEntity>().ToList();
            }

            public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
            {
                return Context.Set<TEntity>().Where(predicate);
            }

            public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
            {
                return Context.Set<TEntity>().SingleOrDefault(predicate);
            }

            public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
            {
                return Context.Set<TEntity>().AsNoTracking().Any(predicate);
            }

            public virtual bool Add(TEntity entity)
            {
                try { Context.Set<TEntity>().Add(entity); }
                catch { return false; }
                return true;
            }
            public virtual bool Update(TEntity entity)
            {
                try { Context.Set<TEntity>().Update(entity); }
                catch{ return false; }
                return true;
            }

            public virtual bool AddRange(IEnumerable<TEntity> entities)
            {
                try { Context.Set<TEntity>().AddRange(entities); }
                catch { return false; }
                return true;
            }

            public virtual bool Remove(TEntity entity)
            {
                try { Context.Set<TEntity>().Remove(entity); }
                catch { return false; }
                return true;
            }

            public virtual bool RemoveRange(IEnumerable<TEntity> entities)
            {
                try { Context.Set<TEntity>().RemoveRange(entities); }
                catch { return true; }
                return false;
            }

            public virtual int Save()
            {
                return Context.SaveChanges();
            }
        }

        #endregion

        #region Course Repository

        public interface ICourseRepository : IRepository<Course>
        {
            IEnumerable<Course> GetTopSellingCourses(int count);
            IEnumerable<Course> GetCoursesWithAuthors(int pageIndex, int pageSize);
        }

        public class CourseRepository : RepositoryBase<Course>, ICourseRepository
        {
            public CourseRepository(CustomContext context) : base(context) { }

            public CustomContext CastedContext => Context as CustomContext;

            public IEnumerable<Course> GetTopSellingCourses(int count)
            {
                return CastedContext.Courses.OrderByDescending(c => c.FullPrice).Take(count).ToList();
            }

            public IEnumerable<Course> GetCoursesWithAuthors(int pageIndex, int pageSize = 10)
            {
                return CastedContext.Courses
                    .Include(c => c.Author)
                    .OrderBy(c => c.Name)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }       

        #endregion

        #region Author Repository

        public interface IAuthorRepository : IRepository<Author>
        {
            Author GetAuthorWithCourses(int id);
        }

        public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
        {
            public AuthorRepository(CustomContext context) : base(context) { }

            public CustomContext CastedContext => Context as CustomContext;

            public Author GetAuthorWithCourses(int id)
            {
                return CastedContext.Authors.Include(a => a.Courses).SingleOrDefault(a => a.Id == id);
            }
        }

        #endregion

        #region Client

        public interface IUnitOfWork : IDisposable
        {
            ICourseRepository Courses { get; }
            IAuthorRepository Authors { get; }
            int Complete();
        }

        public class UnitOfWork : IUnitOfWork
        {
            private readonly CustomContext _context;

            public UnitOfWork(CustomContext context)
            {
                _context = context;
                Courses = new CourseRepository(_context);
                Authors = new AuthorRepository(_context);
            }

            public ICourseRepository Courses { get; private set; }
            public IAuthorRepository Authors { get; private set; }

            public int Complete()
            {
                return _context.SaveChanges();
            }

            public void Dispose()
            {
                _context.Dispose();
            }
        }

        [Test]
        public void Try()
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork(new CustomContext()))
            {
                // Example1
                var course = unitOfWork.Courses.Get(1);

                // Example2
                var courses = unitOfWork.Courses.GetCoursesWithAuthors(1, 4);

                // Example3
                var author = unitOfWork.Authors.GetAuthorWithCourses(1);
                unitOfWork.Courses.RemoveRange(author.Courses);
                unitOfWork.Authors.Remove(author);
                unitOfWork.Complete();
            }
        }

        #endregion

    }
}
