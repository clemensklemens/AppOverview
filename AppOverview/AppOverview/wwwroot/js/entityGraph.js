window.entityGraph = {
  render: function (elementId, nodes, edges) {
    const container = document.getElementById(elementId);
    if (!container) return;
    // Clear container
    container.innerHTML = "";
    // Prepare vis-network data
    // Add formatted label for each node
    const formattedNodes = nodes.map(n => ({
      ...n,
      label: `${n.label}\n${n.department}\n${n.type}${n.description ? `\n${n.description}` : ''}`,
      title: `${n.label}\nDepartment: ${n.department}\nType: ${n.type}\nOwner: ${n.owner}\nURL: ${n.url}`
    }));
    const data = {
      nodes: new vis.DataSet(formattedNodes),
      edges: new vis.DataSet(edges)
    };
    // Set options for better visualization
    const options = {
      nodes: {
        shape: 'box',
        margin: 10,
        font: {
          size: 16,
          multi: false,
          vadjust: 0
        },
        color: {
          background: undefined, // will be set per node
          border: '#222',
          highlight: {
            background: undefined,
            border: '#222'
          }
        }
      },
      edges: {
        arrows: {
          to: { enabled: true, scaleFactor: 2 }
        },
        color: { color: '#848484' },
        length: 300
      },
      physics: {
        stabilization: true
      }
      // Tooltip is handled by vis-network via the 'title' property
    };
    // Set node colors if provided
    data.nodes.forEach(function(node) {
      if (node.color) {
        node.color = {
          background: node.color,
          border: '#222',
          highlight: {
            background: node.color,
            border: '#222'
          }
        };
      }
    });
    // Create network
    new vis.Network(container, data, options);
  }
};
