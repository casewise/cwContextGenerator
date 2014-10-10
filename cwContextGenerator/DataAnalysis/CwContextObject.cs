using Casewise.GraphAPI.API;
using Casewise.GraphAPI.API.Graph;
using cwContextGenerator.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{

    public class Compare
    {
        public cwLightObject SourceObject{get;set;}
        public CwShape TargetShape { get; set; }
        public bool State
        {
            get
            {
                if (this.SourceObject != null & this.TargetShape != null)
                {
                    return true;
                }
                else { return false; }
            }
        }
        public Compare() { }
    }


    public class CwContextObjectParameters
    {
        public int Level { get; set; }
        public CwShape FromShape { get; set; }
        public cwLightObject FromObject { get; set; }
        public CwContextMataModelManager ContextMetaModel { get; set; }
        public ConfigurationRootNode RootConfigurationNode { get; set; }
        public CwDiagram Diagram { get; set; }
        public ConfigurationObjectNode ChildNode { get; set; }
        public cwLightObject ParentContextObject { get; set; }
    }


    public class CwContextObject
    {
        #region properties script name
        protected static string PropertyTypeRootLevel = "ROOTLEVEL";
        protected static string PropertyTypeAtName = "ATNAME";
        protected static string PropertyTypeAtScriptName = "ATSCRIPTNAME";
        protected static string PropertyTypeName = "NAME";
        protected static string[] PropertiesToBySelected = new string[] { PropertyTypeName, PropertyTypeRootLevel, PropertyTypeAtName, PropertyTypeAtScriptName };
        #endregion

        private List<CwShape> _targetShapes = new List<CwShape>();
        private cwLightObject ParentContextObject { get; set; }

        protected CwContextMataModelManager ContextMetaModel { get; set; }
        protected CwDiagram Diagram { get; set; }

        private ConfigurationObjectNode ChildNode { get; set; }
        private cwLightNodeAssociationType AtNode { get; set; }

        public cwLightObject ContextContainer { get; set; }
        protected int Id { get; set; }

        public int Level { get; set; }

        protected cwLightObject FromObject { get; set; }
        protected CwShape FromShape { get; set; }

        public List<cwLightObject> ToObjects { get; set; }
        public List<CwShape> ToShapes { get; set; }
     
        /// <summary>
        /// Context Object Name
        /// </summary>
        protected virtual string Name
        {
            get
            {
                return String.Format("{0}_{1}_L{2}_{3}_{4}_{5}",
                                    Diagram.Type.ToString(),
                                    Diagram.ToString(),
                                    Level.ToString(),
                                    FromObject.ToString(),
                                    ChildNode.ReadingMode.ToString(),
                                    this.Id.ToString());
            }
            set
            {
            }
        }

        #region Constructor
        public CwContextObject(int level, cwLightObject fromObject, CwShape fromShape, CwContextMataModelManager contextMetaModel, CwDiagram diagram)
        {
            this.ContextMetaModel = contextMetaModel;
            this.Diagram = diagram;
            this.FromObject = fromObject;
            this.FromShape = fromShape;
            this.Level = level;
        }


        public CwContextObject(CwContextObjectParameters parameters)
        {
            this.ParentContextObject = parameters.ParentContextObject;
            this.ChildNode = parameters.ChildNode;
            this.AtNode = parameters.ChildNode.GetNode();
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

        public CwContextObject(int level, cwLightObject fromObject, CwShape fromShape, CwContextMataModelManager contextMetaModel, ConfigurationObjectNode childNode, cwLightObject parentContextObject, CwDiagram diagram)
            : this(level, fromObject, fromShape, contextMetaModel, diagram)
        {
            this.ParentContextObject = parentContextObject;

            this.ChildNode = childNode;
            this.AtNode = childNode.GetNode();

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
        private void GetToShapesAndToObjects()
        {
            this.SetTargetShapes();
            this.UnionTargetObjectsAndTargetShapes();
        }

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

        private void UnionTargetObjectsAndTargetShapes()
        {
            Dictionary<string, Compare> compareShapeAndObject = new Dictionary<string, Compare>();
    
            List<cwLightObject> targetObjects = AtNode.getAllTargetObjectsDistinct();

            foreach (CwShape toShape in this._targetShapes)
            {
                string key = toShape.ObjectTypeId.ToString() + toShape.ObjectId.ToString();

                if (!compareShapeAndObject.ContainsKey(key))
                {
                    compareShapeAndObject[key] = new Compare();
                }
                compareShapeAndObject[key].TargetShape = toShape;
            }

            foreach (cwLightObject targetObject in targetObjects)
            {
                string key = targetObject.OTID.ToString() + targetObject.ID.ToString();

                if (!compareShapeAndObject.ContainsKey(key))
                {
                    compareShapeAndObject[key] = new Compare();
                }
                compareShapeAndObject[key].SourceObject = targetObject;
            }

            foreach (var compare in compareShapeAndObject)
            {
                if ( compare.Value.State==true)
                {
                    this.ToObjects.Add(compare.Value.SourceObject);
                    this.ToShapes.Add(compare.Value.TargetShape);
                }
            }
        }
        #endregion

    }
}