

import React, { Component } from 'react'

class Home extends Component {
  render() {
    return (
      <div style={{ textAlign: "center", marginTop: "40px" }}>
      <h1>🏠 Welcome to School Management System </h1>
       <img src="/8-school.png"  alt="School"   style={{
        marginTop: "150px",
        width: "60%",
        maxWidth: "500px",
        borderRadius: "8px"
      }} />
      </div>
    );
  }
}

export default Home