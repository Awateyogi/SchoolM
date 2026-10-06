/*import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Students from './pages/Students';

function App() {
  return (
    <Router>
      <div>
        <nav>
          <Link to="/">Home</Link> |{" "}
          <Link to="/students">Students</Link>
        </nav>
        <Routes>
          <Route path="/" element={<h1>Welcome to School Management</h1>} />
          <Route path="/students" element={<Students />} />
        </Routes>
      </div>
    </Router>
  );
}


  

export default App*/
import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";

// Pages
import Home from './pages/Home';
import Login from './pages/Login';
import Students from './pages/Students';
import AddStudent from './pages/AddStudent';
import EditStudent from './pages/EditStudent';
import Marks from './pages/Marks';
import StudentMarks from "./pages/StudentMarks";
import Marksheet from "./pages/Marksheet";
import StudentFees from "./pages/StudentFees";
import Attendance from "./pages/Attendance";
import './pages/Navbar.css';
 

function App() {
  return (
     <Router>
      <nav className="navbar">
        <Link to="/">Home</Link>

        <div className="dropdown">
            <Link to = "/">Student ▾</Link>
            <div className="dropdown-content">
             <Link to="/add">Add Student</Link>
             <Link to="/students">Students List </Link>
             <Link to="/fees">Student Fees</Link>
             <Link to="/attendance">Student Attendance</Link>
          </div>
        </div>

        <div className="dropdown">
          <Link to="/">Marks ▾</Link>
          <div className="dropdown-content">
            <Link to="/studentmarks">Add Marks</Link>
            <Link to="/marksheet">Student Marksheet</Link>
          </div>
        </div>
      </nav>

      <div style={{ padding: "20px" }}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/students" element={<Students />} />
          <Route path="/add" element={<AddStudent />} />
          <Route path="/edit-student/:id" element={<EditStudent />} />
          <Route path="/studentmarks" element={<StudentMarks />} />
          <Route path="/marksheet" element={<Marksheet />} />
          <Route path="/fees" element={<StudentFees />} />
          <Route path="/attendance" element={<Attendance/>} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;

