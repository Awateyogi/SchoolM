import React, { useEffect, useState } from "react";
import Select from "react-select";
import api from "../Api"; // axios instance
import { Link } from "react-router-dom";
import "./StudentFees.css";


export default function StudentFees() {
  const [students, setStudents] = useState([]);
  const [selectedStudent, setSelectedStudent] = useState(null);
  const [fees, setFees] = useState([]);
  const [classes, setClasses] = useState([]);
  const [selectedClass, setSelectedClass] = useState(null);

  const [amount, setAmount] = useState("");
  const [paid, setPaid] = useState("");
   const [paidAt, setPaidAt] = useState("");
    const [remainingBalance, setRemainingBalance] = useState(null);
   
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


  // Load Students
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
    fetchStudents();
    setSelectedStudent(null);
    setFees([]);
  }, [selectedClass]);

  const handleClassChange = (selected) => {
    setSelectedClass(selected);
    setSelectedStudent(null);
    
  };
  
  const handleStudentChange = (selected) => {
    setSelectedStudent(selected);
  };

  // Load Fees when student selected
  
    const fetchFees = async (studentId, classId) => {
      try {
        const res = await api.get(`/fees/by-student-class?studentId=${selectedStudent.value}&classId=${selectedClass.value}`);
        //setFees(res.data);
        
        const data = res.data; // ✅ define data from response
      setFees(data);

      // 🧠 Auto-fill total amount from first record
      if (data.length > 0) {
        setAmount(data[0].amount || "");
      } else {
        setAmount(""); // clear if no records found
      }

      if (data.length > 0) {
        const totalAmount = data[0].amount || 0;
        const totalPaid = data.reduce((sum, f) => sum + (f.paidAmount || 0), 0);
        const balance = totalAmount - totalPaid;
        setRemainingBalance(balance.toFixed(2));
      } else {
        setRemainingBalance(null);
      }

      } catch (err) {
        console.error("Failed to fetch fees:", err);
        setFees([]);
        setAmount("");
        setRemainingBalance(null);
      }
    };
     useEffect(() => {
  if (selectedStudent && selectedClass) {
    fetchFees(selectedStudent.value, selectedClass.value);
  }
    
  }, [selectedStudent, selectedClass]);

  // Add Fee
 const handleAddFee = async () => {
  try {
    if (!selectedStudent || !selectedClass) {
      alert("Please select both a class and a student.");
      return;
    }

    const paidAtIso = paidAt
      ? new Date(paidAt).toISOString()
      : new Date().toISOString();

    const payload = {
      studentId: selectedStudent.value,
      classId: selectedClass.value,
      amount: parseFloat(amount),
      paidAmount: parseFloat(paid),
      paidAt: paidAtIso,
    };

    console.log("➡️ Sending payload:", payload);
    const res = await api.post("/fees", payload);

    if (res.status === 200 || res.status === 201) {
      alert("✅ Fee record added!");
    } else {
      console.warn("⚠️ Unexpected response:", res);
      alert("Something unexpected happened while saving the fee.");
    }

    // clear inputs
    setAmount("");
    setPaid("");
    setPaidAt("");

    // reload fees
    await fetchFees(selectedStudent.value, selectedClass.value);
  } catch (err) {
    console.error("❌ Error adding fee:", err);

    // safely check response
    const status = err.response?.status;
    const message = err.response?.data || err.message;

    alert(`Failed to add fee (${status ?? "no response"}): ${message}`);
  }
};


  return (
   <div className="fees-container" style={{ padding: "20px" }}>
  <h2>💰 Student Fees</h2>

  {/* Select Class & Student */}
  <div className="dropdown-section" style={{ marginBottom: "20px" }}>
    <div>
      <label>Select Class: </label>
      <Select
        options={classes}
        value={selectedClass}
        onChange={setSelectedClass}
        placeholder="Select Class"
        styles={{ container: base => ({ ...base, width: 200 }) }}
      />
    </div>

    
      <div>
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

  {/* Add Fee Section */}
  {selectedStudent && selectedClass && (
    <div className="fees-form" style={{ marginBottom: "20px" }}>
      {remainingBalance === "0.00" ? (
        <div style={{ color: "green", fontWeight: "bold" }}>
          ✅ This student has completed all fee payments.
        </div>
      ) : (
        <>
          <h4>Add Fee</h4>
          <div className="fees-form-row">
            <input
              type="number"
              placeholder="Total Amount"
              value={amount}
              onChange={e => setAmount(e.target.value)}
            />
            <input
              type="number"
              placeholder="Paid Amount"
              value={paid}
              onChange={e => setPaid(e.target.value)}
            />
            <input
              type="date"
              value={paidAt}
              onChange={e => setPaidAt(e.target.value)}
            />
            <button onClick={handleAddFee}>Add Fee</button>
          </div>
        </>
      )}
    </div>
  )}

  {/* Fee Table */}
  
      <h3>Fee Records</h3>
      
        <table
          className="fees-table"
          border="1"
          cellPadding="6"
          style={{ width: "80%", borderCollapse: "collapse" }}
        >
          <thead>
            <tr>
              <th>FeeId</th>
              <th>Amount</th>
              <th>PaidAmount</th>
              <th>PaidAt</th>
            </tr>
          </thead>
          <tbody>
            {fees.length === 0 ? (
        <p>No fee records found.</p>) :""}
      
            {selectedStudent && (
              <>
            {fees.map(f => (
              <tr key={f.feeId}>
                <td>{f.feeId}</td>
                <td>{(f.amount ?? 0).toFixed(2)}</td>
                <td>{(f.paidAmount ?? 0).toFixed(2)}</td>
                <td>{f.paidAt ? new Date(f.paidAt).toLocaleString() : ""}</td>
              </tr>
            ))}
            </>
          )}
          </tbody>
        
        </table>
      

      {remainingBalance !== null && (
        <h4 className="remaining-balance" style={{ marginTop: "10px" }}>
          Remaining Balance:{" "}
          <span style={{ color: remainingBalance === "0.00" ? "green" : "red" }}>
            ₹{remainingBalance}
          </span>
        </h4>
      )}
    

</div>

)
}