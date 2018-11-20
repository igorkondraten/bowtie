﻿var myDiagram = null;

window.onload = function init() {
    var $ = go.GraphObject.make;
    myDiagram = $(go.Diagram, "myDiagramBowTie",  // create a Diagram for the DIV HTML element
                  {
                      initialContentAlignment: go.Spot.Center,  // center the content
                      "undoManager.isEnabled": true,  // enable undo & redo
                      "animationManager.isEnabled": false,
                      "SelectionDeleting": selectionDeleting,
                      "SelectionDeleted": selectionDeleted
                  });

    myDiagram.model = go.GraphObject.make(go.TreeModel,
        {
            nodeKeyProperty: "Id",
            nodeParentKeyProperty: "ParentId",
            nodeCategoryProperty: "type"
        });

    // define all of the gradient brushes
    var graygrad = $(go.Brush, "Linear", { 0: "#F5F5F5", 1: "#F1F1F1" });
    var bluegrad = $(go.Brush, "Linear", { 0: "#CDDAF0", 1: "#91ADDD" });
    var yellowgrad = $(go.Brush, "Linear", { 0: "#FEC901", 1: "#FEA200" });
    var lavgrad = $(go.Brush, "Linear", { 0: "#EF9EFA", 1: "#A570AD" });

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

    // Event template
    myDiagram.nodeTemplateMap.add("e",
        $(go.Node, "Auto",
        { isShadowed: true, movable: true },
        // define the node's outer shape
        $(go.Shape, "Ellipse",
          { fill: yellowgrad, stroke: "#D8D8D8" }),
        // define the node's text
        $(go.TextBlock,
          {
              margin: 5,
              font: "bold 13px Helvetica, Arial, sans-serif",
              desiredSize: new go.Size(100, 100),
              textAlign: "center",
              verticalAlignment: go.Spot.Center,
              overflow: go.TextBlock.OverflowEllipsis,
              maxLines: 15,
              editable: true
          },
          new go.Binding("text", "Name").makeTwoWay()),
          {
              contextMenu:     // define a context menu for each node
                $(go.Adornment, "Vertical",
                  $("ContextMenuButton",
                    $(go.TextBlock, "Додати причину"),
                    { click: clickNewHazard }),
                  $("ContextMenuButton",
                    $(go.TextBlock, "Додати наслідок"),
                    { click: clickNewCons })
                )
          }, { selectionAdorned: false, deletable: false })
      );

    // Consequences template
    myDiagram.nodeTemplateMap.add("c",
        $(go.Node, "Auto",
        { isShadowed: true, movable: true },
        // define the node's outer shape
        $(go.Shape, "RoundedRectangle",
          { fill: bluegrad, stroke: "#D8D8D8" }),
        // define the node's text
        $(go.TextBlock,
          {
              margin: 5, font: "13px Helvetica, Arial, sans-serif",
              desiredSize: new go.Size(170, 100),
              textAlign: "center",
              verticalAlignment: go.Spot.Center,
              overflow: go.TextBlock.OverflowEllipsis,
              editable: true,
              maxLines: 15
          },
          new go.Binding("text", "Name").makeTwoWay()),
          {
              contextMenu:     // define a context menu for each node
                $(go.Adornment, "Vertical",
                  $("ContextMenuButton",
                    $(go.TextBlock, "Додати новий бар'єр"),
                    { click: clickNewBarrier }),
                  $("ContextMenuButton",
                    $(go.TextBlock, "Додати наслідок"),
                    { click: clickNewSubCons })
                )
          })
      );

    // Subconsequences template
    myDiagram.nodeTemplateMap.add("sc",
        $(go.Node, "Auto",
        { isShadowed: true, movable: true },
        // define the node's outer shape
        $(go.Shape, "RoundedRectangle",
          { fill: bluegrad, stroke: "#D8D8D8" }),
        // define the node's text
        $(go.TextBlock,
          {
              margin: 5, font: "13px Helvetica, Arial, sans-serif",
              desiredSize: new go.Size(150, 70),
              textAlign: "center",
              verticalAlignment: go.Spot.Center,
              overflow: go.TextBlock.OverflowEllipsis,
              editable: true,
              maxLines: 15
          },
          new go.Binding("text", "Name").makeTwoWay()))
      );

    // Hazards template
    myDiagram.nodeTemplateMap.add("h",
        $(go.Node, "Auto",
        { isShadowed: true, movable: true },
        // define the node's outer shape
        $(go.Shape, "RoundedRectangle",
          { fill: lavgrad, stroke: "#D8D8D8" }),
        // define the node's text
        $(go.TextBlock,
          {
              margin: 5, font: "13px Helvetica, Arial, sans-serif",
              desiredSize: new go.Size(170, 100),
              textAlign: "center",
              verticalAlignment: go.Spot.Center,
              editable: true,
              overflow: go.TextBlock.OverflowEllipsis,
              maxLines: 15
          },
          new go.Binding("text", "Name").makeTwoWay()),
          {
              contextMenu:     // define a context menu for each node
                $(go.Adornment, "Vertical",
                  $("ContextMenuButton",
                    $(go.TextBlock, "Додати новий бар'єр"),
                    { click: clickNewBarrier })
                )
          }
      ));

    // Barriers template
    myDiagram.nodeTemplateMap.add("b",
        $(go.Node, "Auto",
        { isShadowed: true, movable: true },
        // define the node's outer shape
        $(go.Shape, "RoundedRectangle",
          { fill: "#9ACD32", stroke: "#D8D8D8" }),
        // define the node's text
        $(go.TextBlock,
          {
              margin: 5, font: "13px Helvetica, Arial, sans-serif",
              desiredSize: new go.Size(80, 150),
              textAlign: "center",
              verticalAlignment: go.Spot.Center,
              overflow: go.TextBlock.OverflowEllipsis,
              editable: true,
              maxLines: 15
          },
          new go.Binding("text", "Name").makeTwoWay())
      ));

    // Link styles
    myDiagram.linkTemplate =
      $(go.Link,  // the whole link panel
        { selectable: false },
        new go.Binding("routing", "routing"),
        $(go.Shape));  // the link shape

    LoadDiagram(defaultSave);

    // Reconnect nodes when deleting middle one
    function selectionDeleting(e) {
        myDiagram.startTransaction('deleteNode');
        var eventData = myDiagram.model.findNodeDataForKey(rootNodeId);
        e.subject.each(function (p) {
            if (p.part.data.type != "sc") {
                if (p.part.data.type != "b") {
                    p.findTreeChildrenNodes().each(function (n) {
                        myDiagram.remove(n);
                    });
                    deleteParentNodes(p.part.findTreeParentNode());
                } else {
                    var newParentId = p.data.ParentId;
                    var nextNodeData = p.findTreeChildrenNodes().first().data;
                    myDiagram.model.setParentKeyForNodeData(nextNodeData, newParentId);
                    if (newParentId == rootNodeId) {
                        myDiagram.model.setDataProperty(nextNodeData, "routing", go.Link.Normal);
                    }
                }
            }
        })
        myDiagram.commitTransaction('deleteNode');
    }

    // Delete connected barriers when deleting consequence or hazard
    function deleteParentNodes(n) {
        if (n.data.type != "e") {
            deleteParentNodes(n.findTreeParentNode());
            myDiagram.remove(n);
        }        
    }

    function selectionDeleted(e) {
        doubleTreeLayout(myDiagram);
    }

    // Create new barrier
    function clickNewBarrier(e, obj) {
        myDiagram.startTransaction('addNewBarrier');
        var fromnode = obj.part.adornedPart;
        var event = myDiagram.model.nodeDataArray.filter(function (obj) {
            return obj.ParentId == -1;
        });
        var newBarrier = {
            Name: 'Новий бар\'єр',
            Id: guid(),
            ParentId: myDiagram.model.getParentKeyForNodeData(fromnode.data),
            dir: fromnode.data.dir,
            type: "b",
            num: fromnode.data.num,
            routing: (event[0].Id == myDiagram.model.getParentKeyForNodeData(fromnode.data)) ? go.Link.Normal : go.Link.Orthogonal
        };
        myDiagram.model.addNodeData(newBarrier);
        myDiagram.model.setParentKeyForNodeData(fromnode.data, newBarrier.Id);
        myDiagram.model.setDataProperty(fromnode.data, "routing", go.Link.Orthogonal);
        doubleTreeLayout(myDiagram);
        myDiagram.updateAllRelationshipsFromData();
        myDiagram.updateAllTargetBindings();
        myDiagram.commitTransaction('addNewBarrier');
    }

    // Reload diagram from JSON data
    function ReloadDiagram() {
        var json = myDiagram.model.toJson();
        myDiagram.clear();
        myDiagram.model = go.Model.fromJson(json);
        myDiagram.add(
        $(go.Part,
        {
            layerName: "Grid",
            _viewPosition: new go.Point(2, 70)
        },
        $(go.Picture, "/Images/legend.png", { width: 85, height: 245 })
        ));
        doubleTreeLayout(myDiagram);
    }

    // Create new hazard
    function clickNewHazard(e, obj) {
        myDiagram.startTransaction('addNewHazard');
        var fromnode = obj.part.adornedPart;
        var eventData = myDiagram.model.findNodeDataForKey(rootNodeId);
        var newHazard = {
            Name: 'Нова причина',
            Id: guid(),
            ParentId: fromnode.data.Id,
            dir: "left",
            num: eventData.hazards + 1,
            type: "h",
            routing: go.Link.Normal
        }
        myDiagram.model.addNodeData(newHazard);
        myDiagram.model.setDataProperty(eventData, "hazards", eventData.hazards + 1);
        doubleTreeLayout(myDiagram);
        myDiagram.commitTransaction('addNewHazard');
    }

    // Create new consequence
    function clickNewCons(e, obj) {
        myDiagram.startTransaction('addNewConsequence');
        var fromnode = obj.part.adornedPart;
        var eventData = myDiagram.model.findNodeDataForKey(rootNodeId);
        var newCons = {
            Name: 'Новий наслідок',
            Id: guid(),
            ParentId: fromnode.data.Id,
            dir: "right",
            num: eventData.cons + 1,
            type: "c",
            routing: go.Link.Normal
        }
        myDiagram.model.addNodeData(newCons);
        myDiagram.model.setDataProperty(eventData, "cons", eventData.cons + 1);
        doubleTreeLayout(myDiagram);
        myDiagram.commitTransaction('addNewConsequence');
    }

    // Create new subconsequence
    function clickNewSubCons(e, obj) {
        myDiagram.startTransaction('addNewSubConsequence');
        var fromnode = obj.part.adornedPart;
        var newSubCons = {
            Name: 'Новий наслідок',
            Id: guid(),
            ParentId: fromnode.data.Id,
            dir: "right",
            num: 0,
            type: "sc",
            routing: go.Link.Normal
        }
        myDiagram.model.addNodeData(newSubCons);
        doubleTreeLayout(myDiagram);
        myDiagram.commitTransaction('addNewSubConsequence');
    }
    
    // Load diagram model from server and apply it
    function LoadDiagram(n) {
        if (n == 0) {
            var t = {
                Name: diagramName,
                Id: rootNodeId,
                ParentId: -1,
                hazards: 0,
                cons: 0,
                color: yellowgrad,
                type: "e"
            };
            myDiagram.model.addNodeData(t);
            doubleTreeLayout(myDiagram);
            SaveDiagram();
        } else {
            jQuery.getJSON("/api/Saves/" + n, {}, function (json) {
                myDiagram.model = go.Model.fromJson(json.Json);
                doubleTreeLayout(myDiagram);
            });
        }          
         myDiagram.add(
        $(go.Part,
          {
              layerName: "Grid",
              _viewPosition: new go.Point(2, 70)
          },
          $(go.Picture, "/Images/legend.png", { width: 85, height: 245 })
          ));
    }       

    // Export model in JSON
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

    // Generate PNG image
    function MakeImage() {
        var img = myDiagram.makeImage({
            scale: 1,
            background: "rgb(255, 255, 255)"
        });
        document.getElementById("downloadButton").href = img.src;
        document.getElementById("downloadButton").download = diagramName;
    }
}

// Make double tree layout
function doubleTreeLayout(diagram) {
    // Within this function override the definition of '$' from jQuery:
    var $ = go.GraphObject.make;  // for conciseness in defining templates
    diagram.startTransaction("Double Tree Layout");

    // split the nodes and links into two Sets, depending on direction
    var leftParts = new go.Set(go.Part);
    var rightParts = new go.Set(go.Part);
    separatePartsByLayout(diagram, leftParts, rightParts);
    // but the ROOT node will be in both collections

    // create and perform two TreeLayouts, one in each direction,
    // without moving the ROOT node, on the different subsets of nodes and links
    var layout1 =
      $(go.TreeLayout,
        {
            angle: 180,
            arrangement: go.TreeLayout.ArrangementFixedRoots,
            setsPortSpot: false,
            nodeSpacing: 10,
            rowSpacing: 10,
            layerSpacing: 20,
            sorting: go.TreeLayout.SortingAscending,
            comparer: function (va, vb) {
                var da = va.node.data;
                var db = vb.node.data;
                if (da.num < db.num) return -1;
                if (da.num > db.num) return 1;
                return 0;
            }
        });

    var layout2 =
      $(go.TreeLayout,
        {
            angle: 0,
            arrangement: go.TreeLayout.ArrangementFixedRoots,
            setsPortSpot: false,
            nodeSpacing: 10,
            rowSpacing: 10,
            layerSpacing: 20,
            sorting: go.TreeLayout.SortingAscending,
            comparer: function(va, vb) {
                var da = va.node.data;
                var db = vb.node.data;
                if (da.num < db.num) return -1;
                if (da.num > db.num) return 1;
                return 0;
            }
        });

    layout1.doLayout(leftParts);
    layout2.doLayout(rightParts);

    diagram.commitTransaction("Double Tree Layout");
}

// Separate parts into left and right
function separatePartsByLayout(diagram, leftParts, rightParts) {
    var root = diagram.findNodeForKey(rootNodeId);
    if (root === null) return;
    // the ROOT node is shared by both subtrees!
    leftParts.add(root);
    rightParts.add(root);
    // look at all of the immediate children of the ROOT node
    root.findTreeChildrenNodes().each(function (child) {
        // in what direction is this child growing?
        var dir = child.data.dir;
        var coll = (dir === "left") ? leftParts : rightParts;
        // add the whole subtree starting with this child node
        coll.addAll(child.findTreeParts());
        // and also add the link from the ROOT node to this child node
        coll.add(child.findTreeParentLink());
    });
}

// Generate GUID for nodes id
function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}