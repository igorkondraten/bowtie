var myDiagram = null;

window.onload = function init() {
    var $ = go.GraphObject.make;
    myDiagram = $(go.Diagram, "myDiagramBowTie",  // create a Diagram for the DIV HTML element
                  {
                      initialContentAlignment: go.Spot.Center,  // center the content
                      "undoManager.isEnabled": true,  // enable undo & redo
                      "animationManager.isEnabled": false,
                      "draggingTool.dragsTree": true,
                      "commandHandler.deletesTree": true,
                      layout:
                        $(go.TreeLayout,
                          { angle: 90, layerSpacing: 30 })
                  });

    function nodeFillConverter(figure) {
        switch (figure) {
            case "AndGate":
                // right to left so when it's rotated, it goes from top to bottom
                return $(go.Brush, "Linear", { 0: "#EA8100", 1: "#C66D00", start: go.Spot.Right, end: go.Spot.Left });
            case "OrGate":
                return $(go.Brush, "Linear", { 0: "#0058D3", 1: "#004FB7", start: go.Spot.Right, end: go.Spot.Left });
            case "Circle":
                return $(go.Brush, "Linear", { 0: "#009620", 1: "#007717" });
            case "Triangle":
                return $(go.Brush, "Linear", { 0: "#7A0099", 1: "#63007F" });
            default:
                return "whitesmoke";
        }
    }

    myDiagram.nodeTemplate =  // the default node template
      $(go.Node, "Spot",
        { selectionObjectName: "BODY", locationSpot: go.Spot.Center, locationObjectName: "BODY" },
        // the main "BODY" consists of a Rectangle surrounding some text
        $(go.Panel, "Auto",
          { name: "BODY", portId: "" },
          $(go.Shape,
            { fill: $(go.Brush, "Linear", { 0: "#770000", 1: "#600000" }), stroke: null }),
          $(go.TextBlock,
            {
                margin: new go.Margin(2, 10, 1, 10), maxSize: new go.Size(100, NaN),
                stroke: "whitesmoke", font: "10pt Segoe UI, sans-serif",
                editable: true
            },
            new go.Binding("text").makeTwoWay())
        ),  // end "BODY", an Auto Panel
        $("TreeExpanderButton", { alignment: go.Spot.Right, alignmentFocus: go.Spot.Left }),
        $(go.Shape, "LineV",
          new go.Binding("visible", "figure", function (f) { return f !== "None"; }),
          { strokeWidth: 1.5, height: 20, alignment: new go.Spot(0.5, 1, 0, -1), alignmentFocus: go.Spot.Top }),
        $(go.Shape,
          new go.Binding("visible", "figure", function (f) { return f !== "None"; }),
          {
              alignment: new go.Spot(0.5, 1, 0, 5), alignmentFocus: go.Spot.Top, width: 30, height: 30,
              stroke: null
          },
          new go.Binding("figure"),
          new go.Binding("fill", "figure", nodeFillConverter),
          new go.Binding("angle", "figure", function (f) { return (f === "OrGate" || f === "AndGate") ? -90 : 0; })), // ORs and ANDs should point upwards
        $(go.TextBlock,
          new go.Binding("visible", "figure", function (f) { return f !== "None"; }), // if we don't have a figure, don't display any choice text
          {
              alignment: new go.Spot(0.5, 1, 20, 20), alignmentFocus: go.Spot.Left,
              stroke: "black", font: "10pt Segoe UI, sans-serif",
              editable: true
          },
          new go.Binding("text", "choice").makeTwoWay()),
          {
              contextMenu:     // define a context menu for each node
                $(go.Adornment, "Vertical",
                  $("ContextMenuButton",
                    $(go.TextBlock, "Додати І"),
                    { click: clickNew1 }),
                    $("ContextMenuButton",
                    $(go.TextBlock, "Додати АБО"),
                    { click: clickNew2 }),
                    $("ContextMenuButton",
                    $(go.TextBlock, "Додати базову подію"),
                    { click: clickNew3 }),
                    $("ContextMenuButton",
                    $(go.TextBlock, "Додати додаткову подію"),
                    { click: clickNew4 }),
                    $("ContextMenuButton",
                    $(go.TextBlock, "Додати подію, яка на окремій сторінці"),
                    { click: clickNew5 })
                )
          }
      );

    myDiagram.linkTemplate =
      $(go.Link, go.Link.Orthogonal,
        { layerName: "Background", curviness: 20, corner: 5 },
        $(go.Shape,
          { strokeWidth: 1.5 })
      );

    myDiagram.model = go.GraphObject.make(go.TreeModel,
        {
            nodeKeyProperty: "Id",
            nodeParentKeyProperty: "ParentId",
            nodeCategoryProperty: "type"
        });

    myDiagram.addDiagramListener("ViewportBoundsChanged", function (e) {
        var dia = e.diagram;
        dia.startTransaction("fix Parts");
        // only iterates through simple Parts in the diagram, not Nodes or Links
        dia.parts.each(function (part) {
            // and only on those that have the "_viewPosition" property set to a Point
            if (part._viewPosition) {
                part.position = dia.transformViewToDoc(part._viewPosition);
                part.scale = 1 / dia.scale;
            }
        });
        dia.commitTransaction("fix Parts");
    });


    LoadDiagram(defaultSave);

    function clickNew1(e, obj) {
        myDiagram.startTransaction('addNew');
        var fromnode = obj.part.adornedPart;
        var newNode = {
            text: 'Назва',
            Id: guid(),
            ParentId: fromnode.data.Id,
            figure: "AndGate",
            choice: "A00"
        }
        myDiagram.model.addNodeData(newNode);
        myDiagram.commitTransaction('addNew');
    }

    function clickNew2(e, obj) {
        myDiagram.startTransaction('addNew');
        var fromnode = obj.part.adornedPart;
        var newNode = {
            text: 'Назва',
            Id: guid(),
            ParentId: fromnode.data.Id,
            figure: "OrGate",
            choice: "A00"
        }
        myDiagram.model.addNodeData(newNode);
        myDiagram.commitTransaction('addNew');
    }

    function clickNew3(e, obj) {
        myDiagram.startTransaction('addNew');
        var fromnode = obj.part.adornedPart;
        var newNode = {
            text: 'Назва',
            Id: guid(),
            ParentId: fromnode.data.Id,
            figure: "Circle",
            choice: "A00"
        }
        myDiagram.model.addNodeData(newNode);
        myDiagram.commitTransaction('addNew');
    }

    function clickNew4(e, obj) {
        myDiagram.startTransaction('addNew');
        var fromnode = obj.part.adornedPart;
        var newNode = {
            text: 'Назва',
            Id: guid(),
            ParentId: fromnode.data.Id,
            figure: "None",
            choice: "A00"
        }
        myDiagram.model.addNodeData(newNode);
        myDiagram.commitTransaction('addNew');
    }

    function clickNew5(e, obj) {
        myDiagram.startTransaction('addNew');
        var fromnode = obj.part.adornedPart;
        var newNode = {
            text: 'Назва',
            Id: guid(),
            ParentId: fromnode.data.Id,
            figure: "Triangle",
            choice: "A00"
        }
        myDiagram.model.addNodeData(newNode);
        myDiagram.commitTransaction('addNew');
    }

    function LoadDiagram(n) {
        if (n == 0) {
            var t = {
                text: diagramName,
                Id: guid(),
                ParentId: -1,
                figure: "None",
                choice: "G03"
            };
            myDiagram.model.addNodeData(t);
            SaveDiagram();
        } else {
            jQuery.getJSON("/api/Saves/" + n, {}, function (json) {
                myDiagram.model = go.Model.fromJson(json.Json);
            });            
        }
    }

    function ReloadDiagram() {
        var json = myDiagram.model.toJson();
        myDiagram.clear();
        myDiagram.model = go.Model.fromJson(json);
    }

    function SaveDiagram() {
        jQuery("[name='Json']").val(myDiagram.model.toJson());
    }

    jQuery('#saveButton').click(function () {
        SaveDiagram();
    });
    jQuery('.loadButton').click(function (e) {
        LoadDiagram(e.target.id);
    });
    jQuery('#refreshButton').click(function () {
        ReloadDiagram();
    });
    jQuery('#downloadButton').click(function () {
        MakeImage();
    });
    function MakeImage() {
        var img = myDiagram.makeImage({
            scale: 1,
            background: "rgb(255, 255, 255)"
        });
        document.getElementById("downloadButton").href = img.src;
        document.getElementById("downloadButton").download = diagramName;
    }
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}