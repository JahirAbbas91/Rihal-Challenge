using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rihal_Challenge.Data;

namespace Rihal_Challenge.Services
{
    public class DataService
    {
        protected readonly ApplicationDbContext _dbcontext;
        public DataService(ApplicationDbContext _db)
        {
            _dbcontext = _db;
        }
        public  bool TestConnection()
        {
            DbConnection conn = _dbcontext.Database.GetDbConnection();

            try
            {
                conn.Open();   // Check the database connection
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Student Entity Start
        public List<StudentsClass> DisplayStudents()
        {

            var students_list = _dbcontext.students
                   .Join(_dbcontext.countries,
                       st => st.CountryId,
                       ct => ct.Id,
                       (st, ct) => new
                       {
                           Id = st.Id,
                           Name = st.Name,
                           DateOfBirth=st.DateOfBirth,
                           CountryId = st.CountryId,
                           ClassId = st.ClassId,
                           CountryName = ct.Name,
                       }).Join(_dbcontext.classes,
                       st => st.ClassId,
                       cl => cl.Id,
                       (st, cl) => new
                       {
                           Id = st.Id,
                           Name = st.Name,
                           DateOfBirth = st.DateOfBirth,
                           CountryId = st.CountryId,
                           ClassId = st.ClassId,
                           CountryName=st.CountryName,
                           ClassName = cl.ClassName
                       }).ToList();

            List<StudentsClass> objlist = new List<StudentsClass>();
          
            foreach (var item in students_list)
            {
                StudentsClass obj = new StudentsClass();
                obj.Id = item.Id;
                obj.Name = item.Name;
                obj.DateOfBirth = item.DateOfBirth;
                obj.CountryId = item.CountryId;
                obj.ClassId = item.ClassId;
                obj.CountryName = item.CountryName;
                obj.ClassName = item.ClassName;
                objlist.Add(obj);
            }
            _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return objlist;
        }


        public bool InsertStudent(StudentsClass obj)
        {
            _dbcontext.students.Add(obj);
            _dbcontext.SaveChanges();
            return true;
        }

        public async Task<int> InsertFakeStudent(int FakeDataCount)
        {
            List<int> countrylist = new List<int>();
            List<int> classlist = new List<int>();
            countrylist =_dbcontext.countries.Select(cn => cn.Id).ToList();
            classlist = _dbcontext.classes.Select(cn => cn.Id).ToList();
            
            for (int i = 0; i < FakeDataCount; i++)
            {
                Random objrandom = new Random();

                int random_country = objrandom.Next(countrylist.Count);
                int random_class = objrandom.Next(classlist.Count);
                var objstudent = new StudentsClass()
                {
                    Name = Faker.Name.FullName(),
                    DateOfBirth = RandomDateofBirth(),
                    ClassId = classlist[random_class],
                    CountryId = countrylist[random_country],
                    CreatedDate=DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                await _dbcontext.students.AddAsync(objstudent);
            }
            return await _dbcontext.SaveChangesAsync();
        }
        public DateTime RandomDateofBirth()
        {
            DateTime start = new DateTime(2000, 1, 1);
            DateTime end = new DateTime(2016, 12, 31);
            Random gen = new Random();
            int range = ((TimeSpan)(end - start)).Days;
            return start.AddDays(gen.Next(range));
        }
        public bool UpdateStudent(StudentsClass obj)
        {
            _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            var objspecific = _dbcontext.students.FirstOrDefault(sel => sel.Id == obj.Id);
            if (objspecific != null)
            {
                objspecific.Name = obj.Name;
                objspecific.DateOfBirth = obj.DateOfBirth;
                objspecific.ClassId = obj.ClassId;
                objspecific.CountryId = obj.CountryId;
                objspecific.ModifiedDate = obj.ModifiedDate;
                _dbcontext.SaveChanges();
            }
            else
            {
                return false;
            }
            
            return true;
        }

        public bool DeleteStudent(StudentsClass obj)
        {
            _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            var objspecific = _dbcontext.students.FirstOrDefault(sel => sel.Id == obj.Id);
            if (objspecific != null)
            {
                _dbcontext.students.Remove(objspecific);
                _dbcontext.SaveChanges();
            }
            else
            {
                return false;
            }

            return true;
        }
        //Student Entity End

        //Country Entity Start
        public List<CountriesClass> DisplayCountries()
        {
            _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return _dbcontext.countries.ToList();
        }
        public bool InsertCountry(CountriesClass obj)
        {
            var objspecific = _dbcontext.countries.FirstOrDefault(sel => sel.Name == obj.Name);
            if (objspecific == null)
            {
                _dbcontext.countries.Add(obj);
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public async Task<int> InsertFakeCountry(int FakeDataCount)
        {
            for(int i = 0; i < FakeDataCount; i++)
            {
                var objcountry = new CountriesClass()
                {
                    Name = Faker.Country.Name(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                await _dbcontext.countries.AddAsync(objcountry);
            }
            return await _dbcontext.SaveChangesAsync();
        }
        public bool UpdateCountry(CountriesClass obj)
        {
            _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            var objspecific = _dbcontext.countries.FirstOrDefault(sel => sel.Id == obj.Id);
            if (objspecific != null)
            {
                objspecific.Name = obj.Name;
                objspecific.ModifiedDate = obj.ModifiedDate;
                _dbcontext.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool DeleteCountry(CountriesClass obj)
        {
            try
            {
                _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var objstu = _dbcontext.students.FirstOrDefault(sel => sel.CountryId == obj.Id);
                if (objstu == null)
                {
                    var objspecific = _dbcontext.countries.FirstOrDefault(sel => sel.Id == obj.Id);
                    if (objspecific != null)
                    {
                        _dbcontext.countries.Remove(objspecific);
                        _dbcontext.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
        }
        //Country Entity End

        //Class Entity Start
        public List<ClassesClass> DisplayClasses()
        {
            _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return _dbcontext.classes.ToList();
        }

        public bool InsertClass(ClassesClass obj)
        {

            var objspecific = _dbcontext.classes.FirstOrDefault(sel => sel.ClassName == obj.ClassName);
            if (objspecific == null)
            {
                _dbcontext.classes.Add(obj);
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
           
        }


        public bool UpdateClass(ClassesClass obj)
        {
            _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            var objspecific = _dbcontext.classes.FirstOrDefault(sel => sel.Id == obj.Id);
            if (objspecific != null)
            {
                objspecific.ClassName = obj.ClassName;
                objspecific.ModifiedDate = obj.ModifiedDate;
                _dbcontext.SaveChanges();
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool DeleteClass(ClassesClass obj)
        {

            try
            {
                _dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                var objstu = _dbcontext.students.FirstOrDefault(sel => sel.ClassId == obj.Id);
                if (objstu == null)
                {
                    var objspecific = _dbcontext.classes.FirstOrDefault(sel => sel.Id == obj.Id);
                    if (objspecific != null)
                    {
                        _dbcontext.classes.Remove(objspecific);
                        _dbcontext.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch
            {
                return false;
            }
           
           
        }
        //Class Entity End


        //Satistics Data Start

        public List<CountriesClass> StudentGroupByCountry()
        {
           
            var StudentGroup =
                from cnt in _dbcontext.countries
            join st in _dbcontext.students
                on cnt.Id equals st.CountryId
            group cnt by cnt.Name into cntGp
                select
                new CountriesClass
                {
                    Name = cntGp.Key,
                    Count = cntGp.Count()
                };
            return StudentGroup.ToList();
        }


        public List<ClassesClass> StudentGroupByClass()
        {

            var StudentGroup =
                from cnt in _dbcontext.classes
                join st in _dbcontext.students
                    on cnt.Id equals st.ClassId
                group cnt by cnt.ClassName into cntGp
                select
                new ClassesClass
                {
                    ClassName = cntGp.Key,
                    Count = cntGp.Count()
                };
            return StudentGroup.ToList();
        }

        public string StudentsAverageAge()
        {
            List<DateTime> StudentList = new List<DateTime>();
            StudentList = _dbcontext.students.Select(cn => cn.DateOfBirth).ToList();
            string avg_age = "0.0";
            if (StudentList.Count != 0)
            {
                double avgDays = StudentList.Average(dt => (DateTime.Now - dt).TotalDays);
                avg_age = $"{avgDays / 365:0.0}";
            }
            return avg_age;
        }

        //Satistics Data End
    }
}
