import React, { useEffect, useState } from "react";
import api from '../Api';
import { useParams, useNavigate } from "react-router-dom";
import './EditStudent.css';
import Select from "react-select";

const EditStudent = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [student, setStudent] = useState({
    firstName: "",
    lastName: "",
    dateOfBirth: "",
    gender: "",
    email: "",
    phone: "",
    classId: "",
  });

  const [classes, setClasses] = useState([]);
  const [error, setError] = useState("");

  // Fetch student by ID
  useEffect(() => {
    // fetch student details
    const fetchStudent = async () => {
      try {
        const res = await api.get(`/students/${id}`);
        setStudent(res.data);
      } catch (err) {
        setError("Failed to fetch student details.");
        console.error(err);
      }
    };

    // fetch classes
    const fetchClasses = async () => {
      try {
        const res = await api.get("/classes");
        const options = res.data.map((c) => ({
          value: c.classId,
          label: c.name,
        }));
        setClasses(options);
      } catch (err) {
        console.error(err);
      }
    };

    fetchStudent();
    fetchClasses();
  }, [id]);

  const handleChange = (e) => {
    setStudent({ ...student, [e.target.name]: e.target.value });
  };

  const handleClassChange = (selectedOption) => {
    setStudent({ ...student, classId: selectedOption ? selectedOption.value : null });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.put(`/students/${student.studentId}`, student);
      navigate("/students");
    } catch (err) {
      console.error(err);
      setError("Failed to update student.");
    }
  };

  return (
    <div className="edit-student-container">
      <div className="edit-student-box">
        <h2>Edit Student</h2>
        {error && <p style={{ color: "red" }}>{error}</p>}
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            name="firstName"
            placeholder="First Name"
            value={student.firstName}
            onChange={handleChange}
            required
          />
          <input
            type="text"
            name="lastName"
            placeholder="Last Name"
            value={student.lastName}
            onChange={handleChange}
            required
          />
          <input
            type="date"
            name="dateOfBirth"
            value={student.dateOfBirth?.split("T")[0] || ""}
            onChange={handleChange}
          />
          <select name="gender" value={student.gender} onChange={handleChange}>
            <option value="">Select Gender</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
          </select>
          <input
            type="email"
            name="email"
            placeholder="Email"
            value={student.email}
            onChange={handleChange}
          />
          <input
            type="text"
            name="phone"
            placeholder="Phone"
            value={student.phone}
            onChange={handleChange}
          />
          <div style={{ marginLeft:"7px",marginBottom:"10px"}}>
  <label style={{ display: "block", marginBottom: "5px", marginLeft:"5px"}}>Class:</label>
          <Select
            options={classes}
            value={classes.find((c) => c.value === student.classId)}
            onChange={handleClassChange}
            placeholder="Select Class"
            styles={{
              control: (base) => ({  ...base,
                minHeight: "30px",
                height: "30px",
                fontSize: "12px",
                backgroundColor: "white",}),
              valueContainer: (base) => ({ ...base,
                height: "30px",
                padding: "0 6px", }),
              input: (base) => ({  ...base,
                margin: "0",
                padding: "0",}),
              indicatorsContainer: (base) => ({ ...base,
                height: "30px", }),
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
          <button type="submit">Update Student</button>
        </form>
      </div>
    </div>

  );
};


export default EditStudent;
