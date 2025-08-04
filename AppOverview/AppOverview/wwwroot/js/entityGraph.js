window.entityGraph = {
  render: function (elementId, nodes, edges) {
    const container = document.getElementById(elementId);
    if (!container) return;
    // Clear container
    container.innerHTML = "";
    // Prepare vis-network data
    const data = {
      nodes: new vis.DataSet(nodes),
      edges: new vis.DataSet(edges)
    };
    // Set options for better visualization
    const options = {
      nodes: {
        shape: 'dot',
        size: 16,
        font: { size: 16 }
      },
      edges: {
        arrows: 'to',
        color: { color: '#848484' }
      },
      physics: {
        stabilization: true
      }
    };
    // Create network
    new vis.Network(container, data, options);
  }
};
