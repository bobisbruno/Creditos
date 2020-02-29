using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anses.ArgentaC.Comun
{
    [Serializable]
    public abstract class DomainObject
    {
        private Int64 id;
        private List<Int64> idList = new List<Int64>();
        public bool ComposedId = false;


        #region propiedades

        public virtual List<Int64> IdList
        {
            get { return idList; }
            set { idList = value; }
        }

        public virtual void UpdateIdList()
        {
            idList.Clear();
            idList.Add(id);
        }

        public virtual Int64 Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public bool HasEqualId(DomainObject otherObj)
        {
            UpdateIdList();
            otherObj.UpdateIdList();
            if (IdList.Count <= 1 && otherObj.IdList.Count <= 1)
                return (id == otherObj.id);
            else
            {
                if (idList.Count != otherObj.idList.Count)
                    return false;
                else
                {
                    for (int i = 0; i < idList.Count; i++)
                    {
                        if (idList[i] != otherObj.idList[i])
                            return false;
                    }
                    return true;
                }
            }
        }
        public virtual bool Validate()
        {
            return true;
        }
        public string IdString
        {
            get { return Id.ToString(); }
            set { Id = Convert.ToInt64(value); }
        }

        #endregion
    }
}