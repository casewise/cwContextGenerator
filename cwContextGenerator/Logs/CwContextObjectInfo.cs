using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;
using cwContextGenerator.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace cwContextGenerator.Logs
{
    public class CwContextObjectInfo
    {
        private cwLightNodeObjectType _otNode;

        private List<string> _to = new List<string>();
        private List<CwContextObjectInfo> _children = new List<CwContextObjectInfo>();

        private CwContextMataModelManager ContextMetaModel { get; set; }
        private cwLightModel Model { get; set; }

        private List<cwLightObject> ChildrenContextObjects { get; set; }

        private cwLightObject CurrentObject { get; set; }
        private cwLightObject FromObject { get; set; }
        private List<cwLightObject> ToObjects { get; set; }


        public int Id
        {
            get { return this.CurrentObject.ID; }
            // set { }
        }

        public string ContextNode
        {
            get
            {
                return this.CurrentObject.ToString();
            }
            set { }
        }


        public string From
        {
            get
            {
                if (this.FromObject == null) { return null; }
                else { return this.FromObject.ToString(); }
            }
            set
            {
            }
        }

        public List<string> To
        {
            get
            {
                foreach (cwLightObject toObject in this.ToObjects)
                {
                    this._to.Add(toObject.ToString());
                }
                return this._to;
            }
            set { }
        }

        public List<CwContextObjectInfo> Children
        {
            get
            {
                foreach (cwLightObject child in this.ChildrenContextObjects)
                {
                    CwContextObjectInfo log = new CwContextObjectInfo(child, this.Model);
                    this._children.Add(log);
                }
                return this._children;
            }
            set { }
        }

        public CwContextObjectInfo() { }

        public CwContextObjectInfo(cwLightObject rootObject, cwLightModel model)
        {
            this.CurrentObject = rootObject;
            this.Model = model;
            this.ContextMetaModel = new CwContextMataModelManager(model);
            this._otNode = this.LoadOTNode();
            this.SetAtTNodesByAtScriptName();
            this.SetFromObject();
            this.SetToObjects();
            this.SetChildrenContextObjects();
        }

        private StringBuilder JavascriptSerialize()
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            StringBuilder output = new StringBuilder();
            s.Serialize(this, output);
            return output;
        }

        private void XmlSerialize()
        {
            //string endwith = string.Format("_{0:yyyy-MM-dd_hh-mm}.xml", DateTime.Now);
            XmlSerializer writer = new XmlSerializer(typeof(CwContextObjectInfo));
            StringBuilder output = new StringBuilder();
            var path = "logs//" + this.CurrentObject.ToString() + ".xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            writer.Serialize(file, this);
            file.Close();
        }


        public void SetLog()
        {
            //this.XmlSerialize();
              this.JsonSerializeAndUpdateIntoDescriptionField();
        }

        private void JsonSerializeAndUpdateIntoDescriptionField()
        {
            StringBuilder serialize = this.JavascriptSerialize();
            this.CurrentObject.getProperty<CwPropertyMemo>("DESCRIPTION").Value = serialize.ToString();
            this.CurrentObject.updatePropertiesInModel();
        }

        private static List<string> AtNodeScriptNames = new List<string>
        {
            CwContextMataModelManager.AtCwContextStartByAnyObjectScriptName, 
            CwContextMataModelManager.AtCwContextEndWithAnyObjectScriptName,
            CwContextMataModelManager.AtCwContextToCwContextScriptName 
        };

        private Dictionary<string, cwLightNodeAssociationType> ATNodesByAtScriptName { get; set; }

        private void SetAtTNodesByAtScriptName()
        {
            Dictionary<string, cwLightNodeAssociationType> atNodesByAtScriptName = new Dictionary<string, cwLightNodeAssociationType>();
            foreach (cwLightNodeAssociationType AtNode in this._otNode.childrenNodes)
            {
                string key = AtNode.AssociationType.ScriptName;
                atNodesByAtScriptName.Add(key, AtNode);
            }
            ATNodesByAtScriptName = atNodesByAtScriptName;
        }

        private cwLightNodeObjectType LoadOTNode()
        {
            cwLightNodeObjectType OTNode = this.Model.GetObjectTypeNode(CwContextMataModelManager.OtCwContextNodeScriptName);
            OTNode.addPropertiesToSelect(new string[] { "ID", "NAME" });

            foreach (string AtScriptName in AtNodeScriptNames)
            {
                cwLightNodeAssociationType ATNode = OTNode.createAssociationNode(AtScriptName);
                ATNode.addPropertiesToSelect(new string[] { "ID", "NAME" });
            }
            OTNode.preloadLightObjects_Rec();
            return OTNode;
        }

        private void SetFromObject()
        {
            cwLightNodeAssociationType ATNode = ATNodesByAtScriptName[CwContextMataModelManager.AtCwContextStartByAnyObjectScriptName];
            this.FromObject = ATNode.GetTargetsForSource(this.CurrentObject).FirstOrDefault();
        }

        private void SetToObjects()
        {
            cwLightNodeAssociationType ATNode = ATNodesByAtScriptName[CwContextMataModelManager.AtCwContextEndWithAnyObjectScriptName];
            this.ToObjects = ATNode.GetTargetsForSource(this.CurrentObject);
        }

        private void SetChildrenContextObjects()
        {
            cwLightNodeAssociationType ATNode = ATNodesByAtScriptName[CwContextMataModelManager.AtCwContextToCwContextScriptName];
            this.ChildrenContextObjects = ATNode.GetTargetsForSource(this.CurrentObject);
        }
    }
}
