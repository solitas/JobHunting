using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using JobHunting.Model;

namespace JobHunting.Data
{
    public class Repository : IRepository, IDisposable
    {
        private IObjectContainer _database;

        public Repository()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(Recruitment)).CascadeOnUpdate(true);
            _database = Db4oEmbedded.OpenFile(config, "JobHunting.db4o");
        }
        public Repository(IObjectContainer Database)
        {
            this._database = Database;
        }

        public IList<Recruitment> LoadRecruitments()
        {
            IList<Recruitment> recruitments = _database.Query<Recruitment>();
            if (recruitments == null)
            {
                return new List<Recruitment>();
            }
            else
            {
                return recruitments; ;
            }
        }

        public void SaveRecruitments(IEnumerable<Recruitment> recruitments)
        {
            foreach (var recruitment in recruitments)
            {
                _database.Store(recruitment);
            }
        }

        public void Delete(object obj)
        {
            _database.Delete(obj);
        }

        public void Dispose()
        {
            _database.Close();
        }
    }
}
