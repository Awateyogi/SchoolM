

import api from '../Api';
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { Link } from 'react-router-dom';
import Select from "react-select";
import './Students.css';

/*function Students() {
  const [students, setStudents] = useState([]);
  const [classes, setClasses] = useState([]);
  const [divisions, setDivisions] = useState([]);
  const [selectedClass, setSelectedClass] = useState(null);
  const [selectedDivision, setSelectedDivision] = useState(null);
   const navigate = useNavigate();

  // Load classes
  useEffect(() => {
    const fetchClasses = async () => {
      try {
        const res = await api.get("/classes");
        const options = res.data.map(c => ({
          value: c.classId,
          label: c.name
        }));
        setClasses(options);
      } catch (err) {
        console.error("Failed to fetch classes:", err);
      }
    };
    fetchClasses();
  }, []);

  


  // Load divisions when class changes
  useEffect(() => {
    if (selectedClass) {
      api.get(`/divisions/by-class/${selectedClass.value}`).then((res) => {
        setDivisions(res.data.map(d => ({ value: d.divisionId, label: d.name })));
      });
    } else {
      setDivisions([]);
      setSelectedDivision(null);
    }
  }, [selectedClass]);

  const handleFilter = async () => {
    if (!selectedClass || !selectedDivision) {
      alert("Please select both Class and Division");
      return;
    }
    try {
      const res = await api.get(
        `/students/by-class-division?classId=${selectedClass.value}&divisionId=${selectedDivision.value}`
      );
      setStudents(res.data);
    } catch (err) {
      console.error("Failed to fetch students:", err);
      setStudents([]);
    }
  };

  const handleUpdate = (studentId) => {
  navigate(`/edit-student/${studentId}`);
};

const handleDelete = async (id) => {
  if (window.confirm("Are you sure you want to delete this student?")) {
    try {
      await api.delete(`/students/${id}`);
      setStudents(students.filter(s => s.studentId !== id));
      alert("Student deleted successfully!");
    } catch (err) {
      console.error("Failed to delete student:", err);
      alert("Failed to delete student.");
    }
  }
};

  return (
    <div>
      <h2>Students</h2>
      <div style={{ display: "flex", gap: "10px", marginBottom: "20px" }}>
        <label style={{fontSize:"16", marginTop:"5px"}}>Class : </label>
         <Select
        options={classes}
        value={selectedClass}
        onChange={setSelectedClass}
        placeholder="Select Class"
      />
      <label style={{fontSize:"16", marginTop:"5px"}}>Division :</label>
      <Select
        options={divisions}
        value={selectedDivision}
        onChange={setSelectedDivision}
        placeholder="Select Division"
        isDisabled={!selectedClass}
      />
        <button onClick={handleFilter}>Filter</button>
      </div>

      <table  border="1" cellPadding="5" cellSpacing="0">
        <thead>
          <tr>
            <th>StudentId</th>
            <th>Name</th>
            <th>Email</th>
            <th>Phone</th>
            <th>DOB</th>
            <th>Gender</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {students.length > 0 ? (
            students.map((s) => (
              <tr key={s.studentId}>
                <td>{s.studentId}</td>
                <td>{s.firstName} {s.lastName}</td>
                <td>{s.email}</td>
                <td>{s.phone}</td>
                <td>{new Date(s.dateOfBirth).toLocaleDateString()}</td>
                <td>{s.gender}</td>
                <td>
                <button onClick={() => handleUpdate(s.studentId)}>Edit</button>
                 &nbsp;
                 <button onClick={() => handleDelete(s.studentId)}>Delete</button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="7">No students found</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
} */


function Students() {
  const [classes, setClasses] = useState([]);
  const [divisions, setDivisions] = useState([]);
  const [selectedClass, setSelectedClass] = useState(null);
  const [selectedDivision, setSelectedDivision] = useState(null);
  const [students, setStudents] = useState([]);
  const [showAll, setShowAll] = useState(false);

  // Load classes
  useEffect(() => {
    const fetchClasses = async () => {
      try {
        const res = await api.get("/api/classes");
        setClasses(res.data.map(c => ({ value: c.classId, label: c.name })));
      } catch (err) {
        console.error("Failed to fetch classes", err);
      }
    };
    fetchClasses();
  }, []);

  // Load divisions when class changes
  useEffect(() => {
    const fetchDivisions = async () => {
      if (!selectedClass) {
        setDivisions([]);
        return;
      }
      try {
        const res = await api.get(`/divisions/by-class/${selectedClass.value}`);
        setDivisions(res.data.map(d => ({ value: d.divisionId, label: d.name })));
      } catch (err) {
        console.error("Failed to fetch divisions", err);
      }
    };
    if (!showAll) fetchDivisions();
  }, [selectedClass, showAll]);

  // Filter students
  const handleFilter = async () => {
    try {
      let url = "/students";

      if (showAll) {
        url = "/students"; // all students
      } else if (selectedClass && !selectedDivision) {
        url = `/students/by-class?classId=${selectedClass.value}`;
      } else if (selectedClass && selectedDivision) {
        url = `/students/by-class-division?classId=${selectedClass.value}&divisionId=${selectedDivision.value}`;
      }

      const res = await api.get(url);
      setStudents(res.data);
    } catch (err) {
      console.error("Failed to fetch students:", err);
      setStudents([]);
    }
  };

  const handleDelete = async (id) => {
  if (window.confirm("Are you sure you want to delete this student?")) {
    try {
      await api.delete(`/students/${id}`);
      setStudents(students.filter(s => s.studentId !== id));
      alert("Student deleted successfully!");
    } catch (err) {
      console.error("Failed to delete student:", err);
      alert("Failed to delete student.");
    }
  }
};

  return (
    <div className="student-container" >
      <h2>Students List</h2>

      {/* Filter controls */}
      <div style={{ display: "flex", gap: "10px", marginBottom: "20px" }}>
        <label>
          <input
            type="checkbox"
            checked={showAll}
            onChange={(e) => {
              setShowAll(e.target.checked);
              setSelectedClass(null);
              setSelectedDivision(null);
            }}
          />
          Show All Students
        </label>

        {!showAll && (
          <>
            <Select
              options={classes}
              value={selectedClass}
              onChange={setSelectedClass}
              placeholder="Select Class"
            />

            <Select
              options={divisions}
              value={selectedDivision}
              onChange={setSelectedDivision}
              placeholder="Select Division"
              isDisabled={!selectedClass}
            />
          </>
        )}

        <button onClick={handleFilter}>Filter</button>
      </div>

      {/* Students table */}
      <table  className="student-table"  border="1" cellPadding="8" style={{ width: "100%", borderCollapse: "collapse" }}>
        <thead>
          <tr>
            <th>StudentId</th>
            <th>Name</th>
            <th>Email</th>
            <th>Phone</th>
            <th>DOB</th>
            <th>Gender</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {students.length > 0 ? (
            students.map((s) => (
              <tr key={s.studentId}>
                <td>{s.studentId}</td>
                <td>{s.firstName} {s.lastName}</td>
                <td>{s.email}</td>
                <td>{s.phone}</td>
                <td>{new Date(s.dateOfBirth).toLocaleDateString()}</td>
                <td>{s.gender}</td>
                <td>
                  <Link to={`/edit-student/${s.studentId}`} className="btn">Edit</Link>
                  {" | "}
                    <button onClick={() => handleDelete(s.studentId)}>Delete</button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="7">No students found</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

export default Students;
