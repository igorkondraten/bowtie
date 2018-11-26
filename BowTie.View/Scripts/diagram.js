var myDiagram = null;
var myDiagramFishBone = null;

window.onload = function init() {
    function initFishBone(){
        var $ = go.GraphObject.make;  // for conciseness in defining templates

        myDiagramFishBone =
          $(go.Diagram, "myDiagramFishBone",  // refers to its DIV HTML element by id
            { isReadOnly: true });  // do not allow the user to modify the diagram

        // define the normal node template, just some text
        myDiagramFishBone.nodeTemplate =
          $(go.Node, "Auto",
             $(go.TextBlock,
            new go.Binding("text"),
            new go.Binding("font", "", convertFont)),
              {
                  contextMenu:
                    $(go.Adornment, "Vertical",
                      $("ContextMenuButton",
                        $(go.TextBlock, "Add new"),
                        { click: clickNewFishboneItem })
                    )
              });

        function convertFont(data) {
            var size = data.size;
            if (size === undefined) size = 13;
            var weight = data.weight;
            if (weight === undefined) weight = "";
            return weight + " " + size + "px sans-serif";
        }

        myDiagramFishBone.linkTemplateMap.add("fishbone",
          $(FishboneLink,  // defined above
            $(go.Shape)
          ));

        // here is the structured data used to build the model
        var json =
          {
              "text": "Incorrect Deliveries", "size": 20, "weight": "Bold", "causes": [
              {
                  "text": "Skills", "size": 18, "weight": "Bold", "causes": [
                  {
                      "text": "knowledge", "weight": "Bold", "causes": [
                      {
                          "text": "procedures", "causes": [
                          { "text": "documentation" }
                          ]
                      },
                      { "text": "products" }
                      ]
                  },
                  { "text": "literacy", "weight": "Bold" }
                  ]
              },
              {
                  "text": "Procedures", "size": 18, "weight": "Bold", "causes": [
                  {
                      "text": "manual", "weight": "Bold", "causes": [
                      { "text": "consistency" }
                      ]
                  },
                  {
                      "text": "automated", "weight": "Bold", "causes": [
                      { "text": "correctness" },
                      { "text": "reliability" }
                      ]
                  }
                  ]
              },
              {
                  "text": "Communication", "size": 18, "weight": "Bold", "causes": [
                  { "text": "ambiguity", "weight": "Bold" },
                  {
                      "text": "sales staff", "weight": "Bold", "causes": [
                      {
                          "text": "order details", "causes": [
                          { "text": "lack of knowledge" }
                          ]
                      }
                      ]
                  },
                  {
                      "text": "telephone orders", "weight": "Bold", "causes": [
                      { "text": "lack of information" }
                      ]
                  },
                  {
                      "text": "picking slips", "weight": "Bold", "causes": [
                      { "text": "details" },
                      { "text": "legibility" }
                      ]
                  }
                  ]
              },
              {
                  "text": "Transport", "size": 18, "weight": "Bold", "causes": [
                  {
                      "text": "information", "weight": "Bold", "causes": [
                      { "text": "incorrect person" },
                      {
                          "text": "incorrect addresses", "causes": [
                          {
                              "text": "customer data base", "causes": [
                              { "text": "not up-to-date" },
                              { "text": "incorrect program" }
                              ]
                          }
                          ]
                      },
                      { "text": "incorrect dept" }
                      ]
                  },
                  {
                      "text": "carriers", "weight": "Bold", "causes": [
                      { "text": "efficiency" },
                      { "text": "methods" }
                      ]
                  }
                  ]
              }
              ]
          };

        function walkJson(obj, arr) {
            var key = guid();
            obj.key = key;
            arr.push(obj);

            var children = obj.causes;
            if (children) {
                for (var i = 0; i < children.length; i++) {
                    var o = children[i];
                    o.parent = key;  // reference to parent node data
                    walkJson(o, arr);
                }
            }
        }

        // build the tree model
        var nodeDataArray = [];
        walkJson(json, nodeDataArray);
        myDiagramFishBone.model = new go.TreeModel(nodeDataArray);

        layoutFishbone();
    }
    function initBowTie() {
        // Bow-Tie Section
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
    }

    initFishBone();
    initBowTie();

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

    function clickNewFishboneItem(e, obj) {
        //myDiagram.startTransaction('addNewFishboneItem');
        var fromnode = obj.part.adornedPart;
        var newCons = {
            text: 'New Item',
            key: guid(),
            parent: fromnode.data.key,
            size: fromnode.data.size > 18 ? fromnode.data.size - 2 : 12,
            weight:  fromnode.data.size >= 18 ? "Bold" : "Normal"
        }
        myDiagramFishBone.model.addNodeData(newCons);
        //layoutFishbone();
        //myDiagram.commitTransaction('addNewFishboneItem');
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

// use FishboneLayout and FishboneLink
function layoutFishbone() {
    myDiagramFishBone.startTransaction("fishbone layout");
    myDiagramFishBone.linkTemplate = myDiagramFishBone.linkTemplateMap.getValue("fishbone");
    myDiagramFishBone.layout = go.GraphObject.make(FishboneLayout, {
        angle: 180,
        layerSpacing: 10,
        nodeSpacing: 20,
        rowSpacing: 10
    });
    myDiagramFishBone.commitTransaction("fishbone layout");
}

/*
*  Copyright (C) 1998-2018 by Northwoods Software Corporation. All Rights Reserved.
*/
/**
* @constructor
* @extends TreeLayout
* @class
* This only works for angle === 0 or angle === 180.
* <p>
* This layout assumes Links are automatically routed in the way needed by fishbone diagrams,
* by using the FishboneLink class instead of go.Link.
*/
function FishboneLayout() {
    go.TreeLayout.call(this);
    this.alignment = go.TreeLayout.AlignmentBusBranching;
    this.setsPortSpot = false;
    this.setsChildPortSpot = false;
}
go.Diagram.inherit(FishboneLayout, go.TreeLayout);

FishboneLayout.prototype.makeNetwork = function (coll) {
    // assert(this.angle === 0 || this.angle === 180);
    // assert(this.alignment === go.TreeLayout.AlignmentBusBranching);
    // assert(this.path !== go.TreeLayout.PathSource);

    // call base method for standard behavior
    var net = go.TreeLayout.prototype.makeNetwork.call(this, coll);
    // make a copy of the collection of TreeVertexes
    // because we will be modifying the TreeNetwork.vertexes collection in the loop
    var verts = new go.List(go.TreeVertex).addAll(net.vertexes);
    verts.each(function (v) {
        // ignore leaves of tree
        if (v.destinationEdges.count === 0) return;
        if (v.destinationEdges.count % 2 === 1) {
            // if there's an odd number of real children, add two dummies
            var dummy = net.createVertex();
            dummy.bounds = new go.Rect();
            dummy.focus = new go.Point();
            net.addVertex(dummy);
            net.linkVertexes(v, dummy, null);
        }
        // make sure there's an odd number of children, including at least one dummy;
        // commitNodes will move the parent node to where this dummy child node is placed
        var dummy2 = net.createVertex();
        dummy2.bounds = v.bounds;
        dummy2.focus = v.focus;
        net.addVertex(dummy2);
        net.linkVertexes(v, dummy2, null);
    });
    return net;
};

FishboneLayout.prototype.assignTreeVertexValues = function (v) {
    go.TreeLayout.prototype.assignTreeVertexValues.call(this, v);
    v._direction = 0;  // add this property to each TreeVertex
    if (v.parent !== null) {
        // The parent node will be moved to where the last dummy will be;
        // reduce the space to account for the future hole.
        if (v.angle === 0 || v.angle === 180) {
            v.layerSpacing -= v.bounds.width;
        } else {
            v.layerSpacing -= v.bounds.height;
        }
    }
};

FishboneLayout.prototype.commitNodes = function () {
    // vertex Angle is set by BusBranching "inheritance";
    // assign spots assuming overall Angle === 0 or 180
    // and links are always connecting horizontal with vertical
    this.network.edges.each(function (e) {
        var link = e.link;
        if (link === null) return;
        link.fromSpot = go.Spot.None;
        link.toSpot = go.Spot.None;

        var v = e.fromVertex;
        var w = e.toVertex;

        if (v.angle === 0) {
            link.fromSpot = go.Spot.MiddleLeft;
        } else if (v.angle === 180) {
            link.fromSpot = go.Spot.MiddleRight;
        }

        if (w.angle === 0) {
            link.toSpot = go.Spot.MiddleLeft;
        } else if (w.angle === 180) {
            link.toSpot = go.Spot.MiddleRight;
        }
    });

    // move the parent node to the location of the last dummy
    this.network.vertexes.each(function (v) {
        var len = v.children.length;
        if (len === 0) return;  // ignore leaf nodes
        if (v.parent === null) return; // don't move root node
        var dummy2 = v.children[len - 1];
        v.centerX = dummy2.centerX;
        v.centerY = dummy2.centerY;
    });

    var layout = this;
    this.network.vertexes.each(function (v) {
        if (v.parent === null) {
            layout.shift(v);
        }
    });

    // now actually change the Node.location of all nodes
    go.TreeLayout.prototype.commitNodes.call(this);
};

// don't use the standard routing done by TreeLayout
FishboneLayout.prototype.commitLinks = function () { };

FishboneLayout.prototype.shift = function (v) {
    var p = v.parent;
    if (p !== null && (v.angle === 90 || v.angle === 270)) {
        var g = p.parent;
        if (g !== null) {
            var shift = v.nodeSpacing;
            if (g._direction > 0) {
                if (g.angle === 90) {
                    if (p.angle === 0) {
                        v._direction = 1;
                        if (v.angle === 270) this.shiftAll(2, -shift, p, v);
                    } else if (p.angle === 180) {
                        v._direction = -1;
                        if (v.angle === 90) this.shiftAll(-2, shift, p, v);
                    }
                } else if (g.angle === 270) {
                    if (p.angle === 0) {
                        v._direction = 1;
                        if (v.angle === 90) this.shiftAll(2, -shift, p, v);
                    } else if (p.angle === 180) {
                        v._direction = -1;
                        if (v.angle === 270) this.shiftAll(-2, shift, p, v);
                    }
                }
            } else if (g._direction < 0) {
                if (g.angle === 90) {
                    if (p.angle === 0) {
                        v._direction = 1;
                        if (v.angle === 90) this.shiftAll(2, -shift, p, v);
                    } else if (p.angle === 180) {
                        v._direction = -1;
                        if (v.angle === 270) this.shiftAll(-2, shift, p, v);
                    }
                } else if (g.angle === 270) {
                    if (p.angle === 0) {
                        v._direction = 1;
                        if (v.angle === 270) this.shiftAll(2, -shift, p, v);
                    } else if (p.angle === 180) {
                        v._direction = -1;
                        if (v.angle === 90) this.shiftAll(-2, shift, p, v);
                    }
                }
            }
        } else {  // g === null: V is a child of the tree ROOT
            var dir = ((p.angle === 0) ? 1 : -1);
            v._direction = dir;
            this.shiftAll(dir, 0, p, v);
        }
    }
    for (var i = 0; i < v.children.length; i++) {
        var c = v.children[i];
        this.shift(c);
    };
};

FishboneLayout.prototype.shiftAll = function (direction, absolute, root, v) {
    // assert(root.angle === 0 || root.angle === 180);
    var locx = v.centerX;
    locx += direction * Math.abs(root.centerY - v.centerY) / 2;
    locx += absolute;
    v.centerX = locx;
    for (var i = 0; i < v.children.length; i++) {
        var c = v.children[i];
        this.shiftAll(direction, absolute, root, c);
    };
};

function FishboneLink() {
    go.Link.call(this);
};
go.Diagram.inherit(FishboneLink, go.Link);

FishboneLink.prototype.computePoints = function () {
    var result = go.Link.prototype.computePoints.call(this);
    if (result) {
        // insert middle point to maintain horizontal lines
        if (this.fromSpot.equals(go.Spot.MiddleRight) || this.fromSpot.equals(go.Spot.MiddleLeft)) {
            var p1;
            // deal with root node being on the "wrong" side
            var fromnode = this.fromNode;
            if (fromnode.findLinksInto().count === 0) {
                // pretend the link is coming from the opposite direction than the declared FromSpot
                var fromport = this.fromPort;
                var fromctr = fromport.getDocumentPoint(go.Spot.Center);
                var fromfar = fromctr.copy();
                fromfar.x += (this.fromSpot.equals(go.Spot.MiddleLeft) ? 99999 : -99999);
                p1 = this.getLinkPointFromPoint(fromnode, fromport, fromctr, fromfar, true).copy();
                // update the route points
                this.setPoint(0, p1);
                var endseg = this.fromEndSegmentLength;
                if (isNaN(endseg)) endseg = fromport.fromEndSegmentLength;
                p1.x += (this.fromSpot.equals(go.Spot.MiddleLeft)) ? endseg : -endseg;
                this.setPoint(1, p1);
            } else {
                p1 = this.getPoint(1);  // points 0 & 1 should be OK already
            }
            var tonode = this.toNode;
            var toport = this.toPort;
            var toctr = toport.getDocumentPoint(go.Spot.Center);
            var far = toctr.copy();
            far.x += (this.fromSpot.equals(go.Spot.MiddleLeft)) ? -99999 / 2 : 99999 / 2;
            far.y += (toctr.y < p1.y) ? 99999 : -99999;
            var p2 = this.getLinkPointFromPoint(tonode, toport, toctr, far, false);
            this.setPoint(2, p2);
            var dx = Math.abs(p2.y - p1.y) / 2;
            if (this.fromSpot.equals(go.Spot.MiddleLeft)) dx = -dx;
            this.insertPoint(2, new go.Point(p2.x + dx, p1.y));
        } else if (this.toSpot.equals(go.Spot.MiddleRight) || this.toSpot.equals(go.Spot.MiddleLeft)) {
            var p1 = this.getPoint(1);  // points 1 & 2 should be OK already
            var fromnode = this.fromNode;
            var fromport = this.fromPort;
            var parentlink = fromnode.findLinksInto().first();
            var fromctr = fromport.getDocumentPoint(go.Spot.Center);
            var far = fromctr.copy();
            far.x += (parentlink !== null && parentlink.fromSpot.equals(go.Spot.MiddleLeft)) ? -99999 / 2 : 99999 / 2;
            far.y += (fromctr.y < p1.y) ? 99999 : -99999;
            var p0 = this.getLinkPointFromPoint(fromnode, fromport, fromctr, far, true);
            this.setPoint(0, p0);
            var dx = Math.abs(p1.y - p0.y) / 2;
            if (parentlink !== null && parentlink.fromSpot.equals(go.Spot.MiddleLeft)) dx = -dx;
            this.insertPoint(1, new go.Point(p0.x + dx, p1.y));
        }
    }
    return result;
};