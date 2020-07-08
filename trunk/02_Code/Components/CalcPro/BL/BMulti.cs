using DataAccess;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BMulti
    {
        DMulti ObjDMulti = new DMulti();

        /// <summary>
        /// Code to get Article groups based on LV section for Multi5 module
        /// </summary>
        /// <param name="ObjEMulti"></param>
        /// <returns></returns>
        public EMulti GetArticleGroups(EMulti ObjEMulti)
        {
            try
            {
               ObjEMulti = ObjDMulti.GetArticleGroups(ObjEMulti);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEMulti;
        }

        /// <summary>
        /// Code to update factors from Multi5 module
        /// </summary>
        /// <param name="ObjEMulti"></param>
        /// <returns></returns>
        public EMulti UpdateMulti5(EMulti ObjEMulti)
        {
            try
            {
               ObjEMulti = ObjDMulti.UpdateMulti5(ObjEMulti);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEMulti;
        }

        /// <summary>
        /// Code to fetch articles groups for Multi6 module
        /// </summary>
        /// <param name="ObjEMulti"></param>
        /// <returns></returns>
        public EMulti GetArticleGroupsForMulti6(EMulti ObjEMulti)
        {
            try
            {
                ObjEMulti = ObjDMulti.GetArticleGroupsForMulti6(ObjEMulti);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEMulti;
        }

        /// <summary>
        /// Code to update factors from multi 6 module
        /// </summary>
        /// <param name="ObjEMulti"></param>
        /// <returns></returns>
        public EMulti UpdateMulti6(EMulti ObjEMulti)
        {
            try
            {
                ObjEMulti = ObjDMulti.UpdateMulti6(ObjEMulti);
            }
            catch (Exception ex)
            {
                throw;
            }
            return ObjEMulti;
        }

        /// <summary>
        /// Code to fetch Sumbitted Multi 5 factors
        /// </summary>
        /// <param name="ObjEMulti"></param>
        /// <returns></returns>
        public EMulti GetSOldMultis(EMulti ObjEMulti)
        {
            try
            {
                ObjDMulti.GetSOldMultis(ObjEMulti);
            }
            catch (Exception ex){}
            return ObjEMulti;
        }

        /// <summary>
        /// Code to fetch Sumbitted Multi 6 factors
        /// </summary>
        /// <param name="ObjEMulti"></param>
        /// <returns></returns>
        public EMulti GetVOldMultis(EMulti ObjEMulti)
        {
            try
            {
                ObjDMulti.GetVOldMultis(ObjEMulti);
            }
            catch (Exception ex) { }
            return ObjEMulti;
        }
    }
}
