import './App.css';
import Header from './Components/Header';
import Home from './Components/Home';
import LoginSignup from './Components/LoginSignup';
import TableScore from './Components/TableScore';
import TableUser from './Components/TableUser';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

function App() {
  return (
  <> 
    {/* <Header /> */}
    <LoginSignup />
    {/* <TableScore /> */}
    {/* <TableUser /> */}

    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/student" element={<TableUser />} />
    </Routes>

  </>
  );
}

export default App;
