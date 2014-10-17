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

        public static string OtCwContextNodeScriptName = "CWCONTEXTNODE";
        public static string AtCwContextEndWithAnyObjectScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASENDOBJECTFORWARDTOANYOBJECT";
        public static string AtCwContextToCwContextScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODECWCONTEXTNODEFATHEROFFORWARDTOCWCONTEXTNODE";
        public static string AtCwContextStartByAnyObjectScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEANYOBJECTHASSTARTOBJECTFORWARDTOANYOBJECT";
        public static string AtCwContextPartOfCwContextPathScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODECWCPATHPARTOFFORWARDTOCWCONTEXTPATH";
        public static string OtCwContextPathScriptName = "CWCONTEXTPATH";
        public static string AtCwContextDescribesDiagramScriptName = "CWCONTEXTNODETOASSOCIATIONCWCONTEXTNODEDIAGRAMDESCRIBESFORWARDTODIAGRAM";

        private cwLightModel Model { get; set; }
        public cwLightObjectType ContextOT
        {
            get { return this.Model.getObjectTypeByScriptName(OtCwContextNodeScriptName); }
        }

        public cwLightObjectType ContextPathOT
        {
            get { return this.Model.getObjectTypeByScriptName(OtCwContextPathScriptName); }
        }

        public cwLightAssociationType AtContextEndWith
        {
            get { return this.ContextOT.getAssociationTypeByScriptName(AtCwContextEndWithAnyObjectScriptName); }
        }

        public cwLightAssociationType AtContextToContext
        {
            get { return this.ContextOT.getAssociationTypeByScriptName(AtCwContextToCwContextScriptName); }
        }

        public cwLightAssociationType AtContextStartFrom
        {
            get { return ContextOT.getAssociationTypeByScriptName(AtCwContextStartByAnyObjectScriptName); }
        }

        public cwLightAssociationType AtContextPartOfPath
        {
            get { return ContextOT.getAssociationTypeByScriptName(AtCwContextPartOfCwContextPathScriptName); }
        }

        public cwLightAssociationType AtContextDiscribesDiagram
        {
            get { return ContextOT.getAssociationTypeByScriptName(AtCwContextDescribesDiagramScriptName); }
        }

        public CwContextMataModelManager(cwLightModel model)
        {
            this.Model = model;
        }
    }
}