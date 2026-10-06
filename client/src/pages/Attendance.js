import React, { useEffect, useState } from "react";
import Select from "react-select";
import api from "../Api";
import "./Attendance.css";

export default function Attendance() {
  const [classes, setClasses] = useState([]);
  const [students, setStudents] = useState([]);
  const [selectedClass, setSelectedClass] = useState(null);
  const [selectedStudent, setSelectedStudent] = useState(null);
  const [date, setDate] = useState(new Date().toISOString().slice(0,10)); // YYYY-MM-DD
  const [status, setStatus] = useState("Present");
  const [notes, setNotes] = useState("");
  const [records, setRecords] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchClasses = async () => {
      try {
        const res = await api.get("/classes");
        setClasses(res.data.map(c => ({ value: c.classId, label: c.name })));
      } catch (err) {
        console.error("Failed to load classes", err);
      }
    }; 
    fetchClasses();
  }, []);

  // when class selected: load students of that class
  useEffect(() => {
    if (!selectedClass) {
      setStudents([]);
      setSelectedStudent(null);
      return;
    }
    const fetchStudents = async () => {
      try {
        const res = await api.get(`/students/by-class?classId=${selectedClass.value}`);
        setStudents(res.data.map(s => ({ value: s.studentId, label: `${s.firstName} ${s.lastName}`, divisionId: s.divisionId })));
      } catch (err) {
        console.error("Failed to load students", err);
      }
    };
    fetchStudents();
  }, [selectedClass]);

  // fetch attendance for class+date
  const fetchClassDateRecords = async () => {
    if (!selectedClass) return;
    setLoading(true);
    try {
      const res = await api.get(`/attendance/by-class-date?classId=${selectedClass.value}&date=${date}`);
      setRecords(res.data);
    } catch (err) {
      console.error("Failed to fetch attendance", err);
      setRecords([]);
    } finally {
      setLoading(false);
    }
  };

  // fetch student attendance
  const fetchStudentRecords = async () => {
    if (!selectedStudent) return;
    setLoading(true);
    try {
      const res = await api.get(`/attendance/student/${selectedStudent.value}`);
      setRecords(res.data);
    } catch (err) {
      console.error("Failed to fetch student attendance", err);
      setRecords([]);
    } finally {
      setLoading(false);
    }
  };

  // add attendance
  const handleAdd = async () => {
    if (!selectedClass || !selectedStudent) {
      alert("Select class and student first");
      return;
    }
    try {
      const payload = {
        studentId: selectedStudent.value,
        classId: selectedClass.value,
        divisionId: selectedStudent.divisionId ?? null,
        date,
        status,
        notes
      };
      await api.post("/attendance", payload);
      alert("Attendance recorded");
      // refresh view: fetch class/date records
      fetchClassDateRecords();
      fetchStudentRecords();
    } catch (err) {
      console.error("Failed to add attendance", err);
      alert("Failed to add attendance (see console)");
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Delete this attendance record?")) return;
    try {
      await api.delete(`/attendance/${id}`);
      // refresh
      fetchClassDateRecords();
      fetchStudentRecords();
    } catch (err) {
      console.error("Failed to delete attendance", err);
      alert("Failed to delete");
    }
  };

  // quick mark present button for class/date (marks all students as present)
  const markAllPresent = async () => {
    if (!selectedClass) {
      alert("Select class first");
      return;
    }
    try {
      const toAdd = students.map(s => ({
        studentId: s.value,
        classId: selectedClass.value,
        divisionId: s.divisionId ?? null,
        date,
        status: "Present",
        notes: null
      }));
      // add one-by-one (or implement batch proc on server)
      for (const p of toAdd) {
        await api.post("/attendance", p);
      }
      alert("Marked all present");
      fetchClassDateRecords();
    } catch (err) {
      console.error(err);
      alert("Error marking all present");
    }
  };

  return (
    <div className="attendance-page">
      <div className="attendance-card">
        <h2>Attendance</h2>

        <div className="row">
          <div className="col">
            <label>Class</label>
            <Select options={classes} value={selectedClass} onChange={setSelectedClass} placeholder="Select Class" />
          </div>

          <div className="col">
            <label>Student</label>
            <Select options={students} value={selectedStudent} onChange={setSelectedStudent} placeholder="Select Student" isDisabled={!selectedClass} />
          </div>

          <div className="col">
            <label>Date</label>
            <input type="date" value={date} onChange={(e) => setDate(e.target.value)} />
          </div>
        </div>

        <div className="row controls">
          <div className="col small">
            <label>Status</label>
            <select value={status} onChange={(e) => setStatus(e.target.value)}>
              <option value="Present">Present</option>
              <option value="Absent">Absent</option>
              <option value="Leave">Leave</option>
            </select>
          </div>

          <div className="col flex-grow">
            <label>Notes</label>
            <input type="text" value={notes} onChange={(e) => setNotes(e.target.value)} placeholder="Optional note" />
          </div>

         <div className="col actions">
           <button onClick={handleAdd}>Add / Update</button>
           <button onClick={fetchClassDateRecords}>View Class / Date</button>
           <button onClick={fetchStudentRecords} disabled={!selectedStudent}>View Student</button>
           <button onClick={markAllPresent} disabled={!selectedClass}>Mark All Present</button>
          </div>

        </div>

        <div className="records">
          <h3>Records</h3>
          {loading ? <div>Loading...</div> :
            records.length === 0 ? (
              <div className="no-records">No records found</div>
            ) : (
              <table className="records-table">
                <thead>
                  <tr>
                    <th>#</th>
                    <th>StudentId</th>
                    <th>Date</th>
                    <th>Status</th>
                    <th>Notes</th>
                    <th>CreatedAt</th>
                    <th>Action</th>
                  </tr>
                </thead>
                <tbody>
                  {records.map((r, i) => (
                    <tr key={r.attendanceId}>
                      <td>{i + 1}</td>
                      <td>{r.studentId} {r.studentName ? `- ${r.studentName}` : ""}</td>
                      <td>{new Date(r.date).toLocaleDateString()}</td>
                      <td>{r.status}</td>
                      <td>{r.notes}</td>
                      <td>{new Date(r.createdAt).toLocaleString()}</td>
                      <td>
                        <button className="btn-delete" onClick={() => handleDelete(r.attendanceId)}>Delete</button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
        </div>
      </div>
    </div>
  );
}
