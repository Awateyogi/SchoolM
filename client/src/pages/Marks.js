import React, { useEffect, useState } from "react";
import api from '../Api';

export default function Marks({ studentId }) {
  const [marks, setMarks] = useState([]);
  const [error, setError] = useState("");

  // Fetch subjects & existing marks for student
  useEffect(() => {
    const fetchData = async () => {
      try {
        const subjRes = await api.get("/subjects"); // 8 subjects
        const markRes = await api.get(`/marks/by-student/${studentId}`);

        const combined = subjRes.data.map((s) => {
          const existing = markRes.data.find((m) => m.subjectId === s.subjectId);
          return {
            subjectId: s.subjectId,
            subjectName: s.name,
            test1: existing?.test1 || "",
            test2: existing?.test2 || "",
            test3: existing?.test3 || "",
            test4: existing?.test4 || "",
            sem1: existing?.sem1 || "",
            sem2: existing?.sem2 || "",
            markId: existing?.markId || null,
          };
        });

        setMarks(combined);
      } catch (err) {
        console.error("Failed to fetch marks:", err);
      }
    };

    fetchData();
  }, [studentId]);

  // Validation function
  const validateMark = (value) => {
    if (value === "" || value.toUpperCase() === "AB") return true; // empty or absent allowed
    const num = Number(value);
    return !isNaN(num) && num >= 0 && num <= 100;
  };

  // Handle input change with validation
  const handleChange = (index, field, value) => {
    if (!validateMark(value)) {
      setError("Marks must be between 0 and 100 or 'AB'.");
      return;
    }
    setError("");
    const updated = [...marks];
    updated[index][field] = value;
    setMarks(updated);
  };

  // Save marks
  const handleSave = async () => {
    try {
      for (const m of marks) {
        if (m.markId) {
          await api.put(`/marks/${m.markId}`, { ...m, studentId });
        } else {
          await api.post("/marks", { ...m, studentId });
        }
      }
      alert("Marks saved successfully!");
    } catch (err) {
      console.error("Failed to save marks:", err);
      alert("Error saving marks.");
    }
  };

  return (
    <div>
      <h2>Enter Marks for Student #{studentId}</h2>

      {error && <p style={{ color: "red" }}>{error}</p>}

      <table border="1" cellPadding="8" style={{ width: "100%", borderCollapse: "collapse" }}>
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
          {marks.map((m, idx) => (
            <tr key={m.subjectId}>
              <td>{m.subjectName}</td>
              {["test1", "test2", "test3", "test4", "sem1", "sem2"].map((field) => (
                <td key={field}>
                  <input
                    type="text"
                    value={m[field]}
                    onChange={(e) => handleChange(idx, field, e.target.value)}
                    placeholder="0-100 or AB"
                  />
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>

      <button onClick={handleSave} style={{ marginTop: "20px" }}>
        Save Marks
      </button>
    </div>
  );
}
