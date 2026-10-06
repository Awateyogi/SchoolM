
import React, { useState, useEffect } from "react";
import Select from "react-select";

import api from '../Api';  // your axios instance
import { useNavigate } from "react-router-dom";
import './AddStudent.css';

/*function AddStudent() {
  // ⚡ Define state for each form field
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("");
  const [gender, setGender] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [classId, setClassId] = useState("");

   const handleSubmit = async (e) => {
  e.preventDefault();

  try {
   await api.post("/students", {
  studentId: 0,              // if needed, backend ignores
  firstName: firstName,
  lastName: lastName,
  dateOfBirth: dateOfBirth,
  gender: gender,
  email: email,
  phone: phone,
  classId: classId ? parseInt(classId) : null
});
    console.log("Student added:", response.data);
  } catch (error) {
    console.error("Failed to add student", error);
  }
};



  return (
    
    <form onSubmit={handleSubmit}>
  <input type="text" value={firstName} onChange={e => setFirstName(e.target.value)} placeholder="First Name" />
  <input type="text" value={lastName} onChange={e => setLastName(e.target.value)} placeholder="Last Name" />
  <input type="date" value={dateOfBirth} onChange={e => setDateOfBirth(e.target.value)} />
  <input type="text" value={gender} onChange={e => setGender(e.target.value)} placeholder="Gender" />
  <input type="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="Email" />
  <input type="text" value={phone} onChange={e => setPhone(e.target.value)} placeholder="Phone" />
  <input type="number" value={classId} onChange={e => setClassId(e.target.value)} placeholder="Class ID" />
  <button type="submit">Add Student</button>
</form>
  );
}
export default AddStudent; */


export default function AddStudent() {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("");
  const [age, setAge] = useState("");
  const [gender, setGender] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [classId, setClassId] = useState(null);
  const [classes, setClasses] = useState([]);
  const [loading, setLoading] = useState(false);
 const navigate = useNavigate();

  // Load available classes on mount
 useEffect(() => {
    const fetchClasses = async () => {
      try {
        const res = await api.get("/classes");
        const options = res.data.map(c => ({
          value: c.classId,
          label: c.name // or c.className
        }));
        setClasses(options);
      } catch (err) {
        console.error("Failed to fetch classes:", err);
      }
    };

    fetchClasses();
  }, []);

 const handleClassChange = (selectedOption) => {
  setClassId(selectedOption ? selectedOption.value : null);
};
  
const handleDateChange = (e) => {
  const dob = e.target.value; // YYYY-MM-DD
  setDateOfBirth(dob);

  if (dob) {
    const [year, month, day] = dob.split('-').map(Number);
    const birthDate = new Date(year, month - 1, day);
    const today = new Date();

    let years = today.getFullYear() - birthDate.getFullYear();
    let months = today.getMonth() - birthDate.getMonth();

    if (today.getDate() < birthDate.getDate()) {
      months--; // not reached birthday this month
    }

    if (months < 0) {
      years--;
      months += 12;
    }

    setAge(`${years} years ${months} months`);
  } else {
    setAge("");
  }
};
  
  const handleSubmit = async (e) => {
  e.preventDefault();
  if (!firstName || !lastName || !gender || !classId) {
    alert("Please fill all required fields!");
    return;
  }

  setLoading(true);
  try {
    await api.post("/students", {
      firstName,
      lastName,
      dateOfBirth,
      gender,
      email,
      phone,
      classId: Number(classId), // Ensure classId is number
    });

    alert("Student added successfully!");
    // Reset all states
    setFirstName("");
    setLastName("");
    setDateOfBirth("");
    setAge("");
    setGender("");
    setEmail("");
    setPhone("");
    setClassId(null);
    navigate("/add");

  } catch (err) {
    console.error("Failed to add student:", err);
    alert("Failed to add student. Check console for details.");
  } finally {
    setLoading(false); // Always reset loading
  }
};

  

  return (
       <div className="add-page">
      <div className="form-card">
        <h2>Add Student</h2>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="First Name"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            required
          />
          <input
            type="text"
            placeholder="Last Name"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            required
          />
          <input
            type="date"
            value={dateOfBirth}
             onChange={handleDateChange}
         
            //onChange={(e) => setDateOfBirth(e.target.value)}
          />
         {age && <p>Age: {age}</p>}
          <div style={{width:"367px"}}>
          <select value={gender} onChange={(e) => setGender(e.target.value)}>
            <option value="">Select Gender</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
          </select>
          </div>
          <input
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <input
            type="text"
            placeholder="Phone"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
          />
          <div style={{marginBottom:"10px", width:"102%"}}>
          <label>Select Class:</label>
          <Select
            options={classes}
            value={classes.find((c) => c.value === classId) || null}
            onChange={handleClassChange}
            placeholder="Select Class"
            styles={{
              control: (base) => ({
                ...base,
                minHeight: "30px",
                height: "30px",
                fontSize: "12px",
                backgroundColor: "white",
              }),
              valueContainer: (base) => ({
                ...base,
                height: "30px",
                padding: "0 6px",
              }),
              input: (base) => ({
                ...base,
                margin: "0",
                padding: "0",
              }),
              indicatorsContainer: (base) => ({
                ...base,
                height: "30px",
              }),
              option: (base, state) => ({
                ...base,
                fontSize: "12px",
                padding: "4px 8px",
                color: state.isSelected ? "white" : "darkblue",
                backgroundColor: state.isSelected ? "blue" : "lightblue",
              }),
              menu: (base) => ({
                ...base,
                fontSize: "12px",
              }),
            }}
          />
          </div>
          <button  type="submit" disabled={loading}> {loading ? "Adding..." : "Add Student"}</button>
        </form>
      </div>
    </div>
  );
}

