using System.Collections.Generic;
using JobHunting.Model;

namespace JobHunting.Data
{
    public interface IRepository
    {
        IList<Recruitment> LoadRecruitments();
        void SaveRecruitments(IEnumerable<Recruitment> recruitments);
        void Delete(object obj);
    }
}
