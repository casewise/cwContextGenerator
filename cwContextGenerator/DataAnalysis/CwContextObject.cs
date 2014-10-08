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
    public class CwContextObject
    {
        protected static string PropertyTypeRootLevel = "ROOTLEVEL";
        protected static string PropertyTypeAtName = "ATNAME";
        protected static string PropertyTypeAtScriptName = "ATSCRIPTNAME";
        protected static string PropertyTypeName = "NAME";
        protected static string[] PropertiesToBySelected = new string[] { PropertyTypeName, PropertyTypeRootLevel, PropertyTypeAtName, PropertyTypeAtScriptName };


        protected CwContextMataModelManager ContextMetaModel { get; set; }
        protected CwDiagram Diagram { get; set; }

        private ConfigurationObjectNode ChildNode { get; set; }
        private cwLightNodeAssociationType AtNode { get; set; }

        public cwLightObject ContextContainer { get; set; }
        public int Id { get; set; }

        public cwLightObject FromObject { get; set; }
        public CwShape FromShape { get; set; }

        public List<cwLightObject> ToObjects { get; set; }
        public List<CwShape> ToShapes { get; set; }

        
        private cwLightObject ParentContextObject { get; set; }

        /// <summary>
        /// Context Object Name
        /// </summary>
        public virtual string Name
        {
            get
            {
                return String.Format("{0}_{1}_{2}_{3}_{4}",
                                    Diagram.Type.ToString(),
                                    Diagram.ToString(),
                                    FromObject.ToString(),
                                    ChildNode.ReadingMode.ToString(),
                                    this.Id.ToString());
            }
            set { }
        }

        public CwContextObject(cwLightObject fromObject, CwShape fromShape, CwContextMataModelManager contextMetaModel, CwDiagram diagram)
        {
            this.ContextMetaModel = contextMetaModel;
            this.Diagram = diagram;
            this.FromObject = fromObject;
            this.FromShape = fromShape;
            this.Create();
        }

        public CwContextObject(cwLightObject fromObject, CwShape fromShape, CwContextMataModelManager contextMetaModel, ConfigurationObjectNode childNode, cwLightObject parentContextObject, CwDiagram diagram)
            : this(fromObject, fromShape, contextMetaModel, diagram)
        {
            this.ParentContextObject = parentContextObject;

            this.ChildNode = childNode;
            this.AtNode = childNode.GetNode();

            this.ToShapes = new List<CwShape>();
            this.ToObjects = new List<cwLightObject>();

            this.GetToShapesAndToObjects();

            this.UpdateProperties();
            this.UpdateAssociations();
        }

        public void Create() {
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

        private void GetToShapesAndToObjects()
        {
            this.SetToShapes();
            this.UnionTargetObjectsAndToShapes();
        }

        private void SetToShapes()
        {
            cwLightAssociationType associationType = AtNode.AssociationType;

            cwLightObjectType targetObjectType = associationType.Target;
            List<CwShape> targetShapes = new List<CwShape>();
            switch (this.ChildNode.ReadingMode)
            {
                case ReadingMode.Includes:
                    this.FromShape.ChildrenShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targetShapes);
                    break;
                case ReadingMode.IsIncludedIn:
                    this.FromShape.ParentsShapesByObjectTypeId.TryGetValue(targetObjectType.ID, out targetShapes);
                    break;
                case ReadingMode.LinkedTo:
                    List<CwShape> shapesLinkedByJoiner = new List<CwShape>();
                    this.FromShape.ToShapesByIntersectionId.TryGetValue(associationType.Intersection.ID, out targetShapes);
                    break;
                default:
                    break;
            }
            this.ToShapes = targetShapes;
        }

        private void UnionTargetObjectsAndToShapes()
        {
            List<string> keys = new List<string>();
            List<cwLightObject> targetObjects = AtNode.getAllTargetObjectsDistinct();

            foreach (CwShape toShape in this.ToShapes)
            {
                string key = toShape.ObjectTypeId.ToString() + toShape.ObjectId.ToString();
                if (!keys.Contains(key))
                {
                    keys.Add(key);
                }
            }

            foreach (cwLightObject targetObject in targetObjects)
            {
                string newKey = targetObject.OTID.ToString() + targetObject.ID.ToString();

                if (keys.Contains(newKey))
                {
                    this.ToObjects.Add(targetObject);
                }
            }
        }

        protected virtual void UpdateAssociations()
        {
            if (this.ToObjects.Count > 0)
            {
                this.ParentContextObject.AssociateToWithTransaction(this.ContextMetaModel.AtContextToContext, this.ContextContainer);
                this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextStartFrom, FromObject);
                foreach (cwLightObject toObject in this.ToObjects)
                {
                    this.ContextContainer.AssociateToWithTransaction(this.ContextMetaModel.AtContextEndWith, toObject);
                }
            }
        }
    }
}