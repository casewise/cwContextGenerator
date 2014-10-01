using Casewise.GraphAPI.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cwContextGenerator.DataAnalysis
{
    public class CwContextMataModelManager
    {

        private static string OtCwContextNodeScriptName = "CWCONTEXTNODE";
        private static string AtCwContextEndWithAnyObjectScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASENDOBJECTFORWARDTOANYOBJECT";
        private static string AtCwContextToCwContextScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODECWCONTEXTNODEFATHEROFFORWARDTOCWCONTEXTNODE";
        private static string AtCwContextStartByAnyObjectScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASSTARTOBJECTFORWARDTOANYOBJECT";
        private static string AtCwContextPartOfCwContextPathScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODECWCPATHPARTOFFORWARDTOCWCONTEXTPATH";
        private static string OtCwContextPathScriptName = "CWCONTEXTPATH";
        private static string AtCwContextDescribesDiagramScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEDIAGRAMDESCRIBESFORWARDTODIAGRAM";


        private cwLightModel Model { get; set; }
        public cwLightObjectType ContextOT
        {
            get
            {
                return this.Model.getObjectTypeByScriptName(OtCwContextNodeScriptName);
            }
        }

        public cwLightObjectType ContextPathOT
        {
            get { return this.Model.getObjectTypeByScriptName(OtCwContextPathScriptName); }
        }
        public cwLightAssociationType AtContextEndWith
        {
            get
            {
                return this.ContextOT.getAssociationTypeByScriptName(AtCwContextEndWithAnyObjectScriptName);
            }
        }

        public cwLightAssociationType AtContextToContext
        {
            get
            {
                return this.ContextOT.getAssociationTypeByScriptName(AtCwContextToCwContextScriptName);
            }
        }

        public cwLightAssociationType AtContextStartFrom
        {
            get
            {
                return ContextOT.getAssociationTypeByScriptName(AtCwContextStartByAnyObjectScriptName);
            }
        }

        public cwLightAssociationType AtContextPartOfPath
        {
            get
            {
                return ContextOT.getAssociationTypeByScriptName(AtCwContextPartOfCwContextPathScriptName);
            }
        }
        public cwLightAssociationType AtContextDiscribesDiagram {
            get {
                return ContextOT.getAssociationTypeByScriptName(AtCwContextDescribesDiagramScriptName);
            }
        }

        public CwContextMataModelManager(cwLightModel model)
        {
            this.Model = model;
        }
    }
}
