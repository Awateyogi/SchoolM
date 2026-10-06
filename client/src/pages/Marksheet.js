
import React, { useState, useEffect } from "react";
import Select from "react-select";
import api from "../Api"; // Axios instance
import "./Marksheet.css"; 

export default function Marksheet() {
  const [classes, setClasses] = useState([]);
  const [selectedClass, setSelectedClass] = useState(null);

  const [students, setStudents] = useState([]);
  const [selectedStudent, setSelectedStudent] = useState(null);

  const [marks, setMarks] = useState([]);
  const [percentage, setPercentage] = useState(0);
  const [divisions, setDivisions] = useState([]);
const [divisionName, setDivisionName] = useState("");

  const [marksstatus, setmarksstatus] = useState("");

  // 1️⃣ Load classes
  useEffect(() => {
    const fetchClasses = async () => {
      try { 
        const res = await api.get("/classes");
        setClasses(
          res.data.map((c) => ({ value: c.classId, label: c.name }))
        );
      } catch (err) {
        console.error("Failed to fetch classes:", err);
      }
    };
    fetchClasses();
  }, []);



  // 2️⃣ Load students by selected class
  useEffect(() => {
    if (!selectedClass) return;

    const fetchStudents = async () => {
      try {
        const res = await api.get(
          `/students/by-class?classId=${selectedClass.value}`
        );
        setStudents(
          res.data.map((s) => ({
            value: s.studentId,
            label: `${s.firstName} ${s.lastName}`,
             divisionId: s.divisionId, // keep divisionId for mapping

          }))
        );
      } catch (err) {
        console.error("Failed to fetch students:", err);
      }
    };

    const fetchDivisions = async () => {
      try {
        const res = await api.get(`/divisions/by-class/${selectedClass.value}`);
        setDivisions(res.data); // { divisionId, name }
      } catch (err) {
        console.error("Failed to fetch divisions:", err);
      }
    };

    fetchStudents();
    fetchDivisions();
  }, [selectedClass]);
  

   const handleStudentChange = (selected) => {
    setSelectedStudent(selected);

    const division = divisions.find(d => d.divisionId === students.divisionId);
  setDivisionName(division ? divisions.name : "");
  };


  // 3️⃣ Load marks by selected student
  useEffect(() => {
    if (!selectedStudent) return;

    // 🔹 Find division name
    const division = divisions.find(
      d => d.divisionId === selectedStudent.divisionId
    );
    setDivisionName(division ? division.name : "");

    const fetchMarks = async () => {
      try {
        const res = await api.get(`/marks/by-student/${selectedStudent.value}`);
        setMarks(res.data);
      }catch (err) {
        console.error("Failed to fetch marks:", err);
        setMarks([]);
      }
    };

    fetchMarks();
  }, [selectedStudent,divisions]);

 const calculatePercentage = () => {
    if (!marks.length) return 0;

    let totalObtained = 0;
    let totalMax = 0;

    marks.forEach(m => {
      const subjectMarks = [
        m.test1 ?? 0,
        m.test2 ?? 0,
        m.test3 ?? 0,
        m.test4 ?? 0,
        m.sem1 ?? 0,
        m.sem2 ?? 0,
      ];
      totalObtained += subjectMarks.reduce((a, b) => a + b, 0);
      totalMax += subjectMarks.length * 100; // each out of 100
    });

    return ((totalObtained / totalMax) * 100).toFixed(2);
  };


  const handleClassChange = (selected) => {
    setSelectedClass(selected);
    setSelectedStudent(null);
    setMarks([]);
    setPercentage(0);

  };

 
  return (
    <div  className="marksheet-container" style={{ padding: "20px" }}>
      <h2>Marksheet</h2>

       <div className="dropdown-section">
      {/* Select Class */}
      <div className="dropdown-item" style={{ marginBottom: "20px" }}>
        <label>Select Class: </label>
        <Select
          options={classes}
          value={selectedClass}
          onChange={handleClassChange}
          placeholder="Select Class"
          styles={{
            container: (base) => ({ ...base, width: 200 }),
          }}
        />
      </div>

      {/* Select Student */}
      
        <div className="dropdown-item" style={{ marginBottom: "20px" }}>
          <label>Select Student: </label>
          <Select
            options={students}
            value={selectedStudent}
            onChange={handleStudentChange}
            placeholder="Select Student"
            styles={{
              container: (base) => ({ ...base, width: 250 }),
            }}
          />
        </div>

        


      <div >
         {selectedStudent && (
 
    <label>
      <strong>Division:</strong> {divisionName}
    </label>

)}
      </div>
      </div>

      {/* Marks Table */}
      
        <div className="table-section" >
          <table className="marksheet-table">
           
            <thead>
              <tr>
                <th>Subject</th>
                <th>Test 1</th>
                <th>Test 2</th>
                <th>Test 3</th>
                <th>Test 4</th>
                <th>Sem 1</th>
                <th>Sem 2</th>
              </tr>
            </thead>
            <tbody>
               {marks.length === 0 ? (
        //<p>No  records found.</p>) :""}
               <tr>
              <td colSpan="7" className="no-records">No records found.</td>
              </tr> ) :""}
              {selectedStudent && marks.length > 0 && (
                <>
                {selectedClass && (
                  <>
              {marks.map((m) => (
                <tr key={m.markId}>
                  <td>{m.subjectName || m.subject}</td>
                  <td>{m.test1 ?? 0}</td>
                  <td>{m.test2 ?? 0}</td>
                  <td>{m.test3 ?? 0}</td>
                  <td>{m.test4 ?? 0}</td>
                  <td>{m.sem1 ?? 0}</td>
                  <td>{m.sem2 ?? 0}</td>
                </tr>
              ))}
            </>)}

           
             </>
            )}
            </tbody>
          </table>

          {/* Percentage and Division */}
           <div className="summary-section">
            <strong>Percentage:</strong> {calculatePercentage()}% 
            
           
          </div>
          
        </div>
  
    </div>
  );
}
