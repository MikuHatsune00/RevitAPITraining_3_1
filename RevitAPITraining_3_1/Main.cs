using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_3_1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedRefList = uidoc.Selection.PickObjects(ObjectType.Face, "Выберите элемент по грани");
            var elementList = new List<Element>();
            string info = string.Empty;
            double xref = 0;
            foreach (var selectedElement in selectedRefList)
            { Element element = doc.GetElement(selectedElement);
                elementList.Add(element);
                if (element is Wall)
                {   
                    Parameter Vparameter = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                    if (Vparameter.StorageType == StorageType.Double)
                    {
                        double VValue = UnitUtils.ConvertFromInternalUnits(Vparameter.AsDouble(), UnitTypeId.CubicMeters);
                        xref += VValue;                 
                       

                    }
                    
                   
                }
               
            }
            info += $"{xref}";
            TaskDialog.Show("Volume", info);

            return Result.Succeeded;
        }
    }
}
