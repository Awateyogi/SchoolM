import React, { useState, useEffect } from "react";
import api from '../Api'; // your Axios instance
import "./StudentMarks.css";
import Select from "react-select";

export default function StudentMarks() {
  const [students, setStudents] = useState([]);
  const [classes, setClasses]=useState([]);
  const [subjectsList, setSubjectsList] = useState([]);
  const [selectedStudent, setSelectedStudent] = useState("");
  const [marks, setMarks] = useState([]);
  const [isNewRecord, setIsNewRecord] = useState(false);
   const [selectedClass, setSelectedClass] = useState("");
 

  // 1️⃣ Load subjects
  useEffect(() => {
    const fetchSubjects = async () => {
      try {
        const res = await api.get("/subjects"); // endpoint must match backend
        setSubjectsList(res.data);
        console.log("Subjects loaded:", res.data);
      } catch (err) {
        console.error("Failed to fetch subjects:", err);
      }
    }; 
    fetchSubjects();
  }, []);


   // Load classes
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

  // 2️⃣ Load students
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
    fetchStudents();
    setSelectedStudent(null);

  }, [selectedClass]);

  const handleClassChange = (selected) => {
    setSelectedClass(selected);
    setSelectedStudent(null);
    
  };
  
  const handleStudentChange = (selected) => {
    setSelectedStudent(selected);
  };


useEffect(() => {
  if (!selectedStudent || subjectsList.length === 0) return;

  const fetchMarks = async () => {
    try {
      const res = await api.get(`/marks/by-student/${selectedStudent.value}`);

      if (res.data.length > 0) {
        // ✅ existing marks → map with subjectId
        setMarks(
          res.data.map(m => ({
            markId: m.markId,
            studentId: selectedStudent.value,
            subjectId: m.subjectId,      // use subjectId, not name
            subjectName: m.subject,
            test1: m.test1 ?? 0,
            test2: m.test2 ?? 0,
            test3: m.test3 ?? 0,
            test4: m.test4 ?? 0,
            sem1: m.sem1 ?? 0,
            sem2: m.sem2 ?? 0,
          }))
        );
      } else {
        // ✅ no data → create empty rows for all subjects
        setMarks(
          subjectsList.map(subj => ({
            markId: 0,
            studentId: selectedStudent.value,
            subjectId: subj.subjectId,
            subjectName: subj.name,
            test1: 0,
            test2: 0,
            test3: 0,
            test4: 0,
            sem1: 0,
            sem2: 0,
          }))
        );
      }
    } catch (err) {
      console.error("Failed to fetch marks:", err);
      setMarks([]);
    }
  };

  fetchMarks();
}, [selectedStudent, subjectsList]);


  // 4️⃣ Handle input changes
  const handleChange = (index, field, value) => {
    const updated = [...marks];
    updated[index][field] = value;
    setMarks(updated);
  };

  // 5️⃣ Save all marks (add/update)
 /* const handleSave = async () => {
  try {
    for (let m of marks) {
      const payload = {
        studentId: Number(selectedStudent), // always a valid student
        subjectId: Number(m.subjectId),     // subject dropdown value
        subject: subjectsList.find(s => s.subjectId === m.subjectId)?.name || "",
        test1: m.test1 ?? null,
        test2: m.test2 ?? null,
        test3: m.test3 ?? null,
        test4: m.test4 ?? null,
        sem1: m.sem1 ?? null,
        sem2: m.sem2 ?? null,
      };

      if (!payload.studentId || !payload.subjectId) {
        alert("⚠ Please select both student and subject.");
        return;
      }

      if (m.markId === 0) {
        await api.post("/marks", payload); // Add new
      } else {
        await api.put(`/marks/${m.markId}`, { markId: m.markId, ...payload }); // Update existing
      }
    }
    alert("Marks saved successfully!");
  } catch (err) {
    console.error("Error saving marks:", err);
    alert("❌ Failed to save marks. Check console.");
  }
};  */

 const handleSave = async () => {
    try {
      for (let m of marks) {
        const payload = {
          studentId: Number(selectedStudent.value),
          subjectId: Number(m.subjectId),
          subject: m.subjectName || "",
          test1: m.test1 ?? null,
          test2: m.test2 ?? null,
          test3: m.test3 ?? null,
          test4: m.test4 ?? null,
          sem1: m.sem1 ?? null,
          sem2: m.sem2 ?? null,
        };

        if (isNewRecord || m.markId === 0) {
          await api.post("/marks", payload); // insert
        } else {
          await api.put(`/marks/${m.markId}`, { markId: m.markId, ...payload }); // update
        }
      }

      alert("✅ Marks saved successfully!");
      setIsNewRecord(false);
    } catch (err) {
      console.error("Error saving marks:", err);
      alert("❌ Failed to save marks. Check console.");
    }
  };

  return (
    <div className="studentmarks-container" style={{ padding: "20px" }}>
      <h2>📖 Student Marks</h2>

      {/* Class and Student dropdowns */}
      <div className="dropdown-section">
         <div className="dropdown-item">
      <label>Select Class: </label>
      <Select
        options={classes}
        value={selectedClass}
        onChange={setSelectedClass}
        placeholder="Select Class"
        styles={{ container: base => ({ ...base, width: 200 }) }}
      />
    </div>

        
          
          
      <div className="dropdown-item">
        <label>Select Student: </label>
        <Select
          options={students}
          value={selectedStudent}
          onChange={setSelectedStudent}
          placeholder="Select Student"
          styles={{ container: base => ({ ...base, width: 250 }) }}
        />
      </div>
  
    </div>
    

      {/* Marks Table */}
     
        <table className="studentmarks-table">
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
         </tr> ):""}
            {selectedStudent && marks.length > 0 && (
            <>
            {selectedClass && (
              <>
            {marks.map((m, idx) => (
              <tr key={idx}>
                <td>
                  <select
                    value={m.subjectId || ""}
                    onChange={(e) => {
                      const updated = [...marks];
                      const subjId = Number(e.target.value);
                      updated[idx].subjectId = subjId;
                      updated[idx].subjectName =
                        subjectsList.find((s) => s.subjectId === subjId)?.name || "";
                      setMarks(updated);
                    }}
                  >
                    <option value="">--Select Subject--</option>
                    {subjectsList.map((s) => (
                      <option key={s.subjectId} value={s.subjectId}>
                        {s.name}
                      </option>
                    ))}
                  </select>
                </td>

                {[1, 2, 3, 4].map((t) => (
                  <td key={t}>
                    <input
                      type="number"
                      min="0"
                      max="100"
                      value={m[`test${t}`]}
                      onChange={(e) =>
                        handleChange(idx, `test${t}`, Number(e.target.value))
                      }
                    />
                  </td>
                ))}

                <td>
                  <input
                    type="number"
                    min="0"
                    max="100"
                    value={m.sem1}
                    onChange={(e) =>
                      handleChange(idx, "sem1", Number(e.target.value))
                    }
                  />
                </td>
                <td>
                  <input
                    type="number"
                    min="0"
                    max="100"
                    value={m.sem2}
                    onChange={(e) =>
                      handleChange(idx, "sem2", Number(e.target.value))
                    }
                  />
                </td>
              </tr>
            ))}
           </>
          )}
           </>
            )}
          </tbody>
        </table>
      

      {/* Save Button */}
      {selectedStudent && marks.length > 0 && (
        <div className="save-btn-container">
          <button onClick={handleSave}>
            {marks.every((m) => m.markId === 0)
              ? "Insert Marks"
              : "Update Marks"}
          </button>
        </div>
      )}
    </div>
  );
  
}
