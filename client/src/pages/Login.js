import { useState } from "react";
import api from '../Api';

function Login() {
  /*const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");

  const login = async () => {
    try {
      const res = await api.post("/auth/login", { userName, passwordHash: password });
      localStorage.setItem("token", res.data.token);
      localStorage.setItem("role", res.data.role);
      window.location.href = "/";
    } catch {
      alert("Invalid credentials");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Login</h2>
      <input value={userName} onChange={e => setUserName(e.target.value)} placeholder="Username" /><br/>
      <input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" /><br/>
      <button onClick={login}>Login</button>
    </div>
  );*/
  return(
   <div>
      <h1>🔑 Login Page</h1>
      <input placeholder="Username" /><br />
      <input type="password" placeholder="Password" /><br />
      <button>Login</button>
    </div>
  );
}

export default Login;
