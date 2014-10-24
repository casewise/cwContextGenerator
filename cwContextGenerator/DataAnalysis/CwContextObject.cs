using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;
using cwContextGenerator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cwContextGenerator.Compare;
namespace cwContextGenerator.DataAnalysis
{




    public class CwContextObjectParameters
    {
        public int Level { get; set; }
        public CwShape FromShape { get; set; }
        public cwLightObject FromObject { get; set; }
        public CwContextMataModelManager ContextMetaModel { get; set; }
        public ConfigurationRootNode RootConfigurationNode { get; set; }
        public CwDiagram Diagram { get; set; }
        // public ConfigurationObjectNode ChildNode { get; set; }
        // public cwLightObject ParentContextObject { get; set; }
    }


    public class CwContextObject
    {
        #region properties script name
        protected static string PropertyTypeRootLevel = "ROOTLEVEL";
        protected static string PropertyTypeAtName = "ATNAME";
        protected static string PropertyTypeAtScriptName = "ATSCRIPTNAME";
        protected static string PropertyTypeName = "NAME";
        protected static string PropertyTypeDescription = "DESCRIPTION";

        protected static string[] PropertiesToBySelected = new string[] { PropertyTypeName, PropertyTypeRootLevel, PropertyTypeAtName, PropertyTypeAtScriptName, PropertyTypeDescription };
        #endregion

        

        private List<CwShape> _targetShapes = new List<CwShape>();
        private cwLightObject ParentContextObject { get; set; }

        protected CwContextMataModelManager ContextMetaModel { get; set; }
        protected CwDiagram Diagram { get; set; }

        private ConfigurationObjectNode ChildNode { get; set; }
        private cwLightNodeAssociationType AtNode { get; set; }
        protected const int ObjectNameMaxLength = 250;

        public cwLightObject ContextContainer { get; set; }
        protected int Id { get; set; }

        public int Level { get; set; }

        protected cwLightObject FromObject { get; set; }
        protected CwShape FromShape { get; set; }

        private List<cwLightObject> ToObjects { get; set; }
        public List<CwShape> ToShapes { get; set; }

        /// <summary>
        /// Context Object Name
        /// </summary>
        protected virtual string Name
        {
            get
            {
                string name = String.Format("{0}_{1}_L{2}_{3}_{4}_[{5}]_",
                                    Diagram.Type.ToString(),
                                    Diagram.ToString(),
                                    Level.ToString(),
                                    FromObject.ToString(),
                                    ChildNode.ReadingMode.ToString(),
                                    this.AtNode.TargetObjectType.ScriptName.ToString());
                if (name.Length > ObjectNameMaxLength) { name = name.Substring(0, ObjectNameMaxLength); }
                name = name + this.Id.ToString();
                return name;
            }
            set
            {
            }
        }

        #region Constructor
        public CwContextObject(ConfigurationObjectNode childNode, cwLightObject parentContextObject, CwContextObjectParameters parameters)
            : this(parameters)
        {

            this.ParentContextObject = parentContextObject;
            this.ChildNode = childNode;
            this.AtNode = this.ChildNode.GetNode();

            this.ToShapes = new List<CwShape>();
            this.ToObjects = new List<cwLightObject>();

            this.GetToShapesAndToObjects();
            if (this.ToShapes.Count > 0)
            {
                this.Create();
                this.UpdateProperties();
                this.UpdateAssociations();
            }
        }

        protected CwContextObject(CwContextObjectParameters parameters)
        {
            this.ContextMetaModel = parameters.ContextMetaModel;
            this.Diagram = parameters.Diagram;
            this.FromObject = parameters.FromObject;
            this.FromShape = parameters.FromShape;
            this.Level = parameters.Level;
        }

        #endregion

        #region create context object and update related data
        protected void Create()
        {
            //new context object, return context object id
            this.Id = this.ContextMetaModel.ContextOT.createObject();

            cwLightNodeObjectType contextOTNode = this.ContextMetaModel.ContextOT.getNewNode();
            contextOTNode.addPropertiesToSelect(PropertiesToBySelected);
            contextOTNode.preloadLightObjects();
            //get contextObject
            this.ContextContainer = contextOTNode.usedOTLightObjectsByID[this.Id];
        }

        protected virtual void UpdateProperties()
        {
            this.ContextContainer.properties[PropertyTypeAtName].Value = AtNode.AssociationType.ToString();
            this.ContextContainer.properties[PropertyTypeAtScriptName].Value = AtNode.AssociationType.ScriptName;
            this.ContextContainer.properties[PropertyTypeName].Value = this.Name;
            this.ContextContainer.updatePropertiesInModel();
        }

        protected virtual void UpdateAssociations()
        {
            this.ParentContextObject.AssociateToWithTransaction(this.ContextMetaModel.AtContextToContext, this.ContextContainer);
            this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextStartFrom, FromObject);
            foreach (cwLightObject toObject in this.ToObjects)
            {
                this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextEndWith, toObject);
            }
        }
        #endregion

        #region Analysis
        /// <summary>
        /// 
        /// </summary>
        private void GetToShapesAndToObjects()
        {
            this.SetTargetShapes();
            this.UnionTargetObjectsAndTargetShapes();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetTargetShapes()
        {
            cwLightAssociationType associationType = AtNode.AssociationType;

            cwLightObjectType targetObjectType = associationType.Target;
            List<CwShape> targetShapes = new List<CwShape>();
            switch (this.ChildNode.ReadingMode)
            {
                case ReadingMode.Directly_Includes:
                    this.FromShape.ChildrenShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targetShapes);
                    break;
                case ReadingMode.Directly_IsIncludedIn:
                    this.FromShape.ParentsShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targetShapes);
                    break;
                case ReadingMode.Includes:
                    this.FromShape.DescendantsShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targetShapes);
                    break;
                case ReadingMode.IsIncludedIn:
                    this.FromShape.AncestorsShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targetShapes);
                    break;
                case ReadingMode.LinkedTo:
                    List<CwShape> shapesLinkedByJoiner = new List<CwShape>();
                    this.FromShape.ToShapesByIntersectionId.TryGetValue(associationType.Intersection.ID, out targetShapes);
                    break;
                default:
                    break;
            }
            if (targetShapes != null)
            {
                this._targetShapes = targetShapes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UnionTargetObjectsAndTargetShapes()
        {
            Dictionary<string, CwObjectShapeMapping> objectShapeMapping = new Dictionary<string, CwObjectShapeMapping>();

            List<cwLightObject> targetObjects = AtNode.getAllTargetObjectsDistinct();

            foreach (CwShape toShape in this._targetShapes)
            {
                string key = toShape.ObjectTypeId.ToString() + toShape.ObjectId.ToString();

                if (!objectShapeMapping.ContainsKey(key))
                {
                    objectShapeMapping[key] = new CwObjectShapeMapping();
                }
                objectShapeMapping[key].TargetShapes.Add(toShape);
            }

            foreach (cwLightObject targetObject in targetObjects)
            {
                string key = targetObject.OTID.ToString() + targetObject.ID.ToString();

                if (!objectShapeMapping.ContainsKey(key))
                {
                    objectShapeMapping[key] = new CwObjectShapeMapping();
                }
                objectShapeMapping[key].SourceObject = targetObject;
            }

            foreach (var map in objectShapeMapping)
            {
                if (map.Value.NoMapping == false)
                {
                    this.ToObjects.Add(map.Value.SourceObject);
                    this.ToShapes.AddRange(map.Value.TargetShapes);
                }
            }
        }
        #endregion
    }
}