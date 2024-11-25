import React from "react";
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import UserNavbar from "../navbars/UserNavbar";
import Home from "../../home/Home";
import Sensors from "../../sensors/SensorsTable";
const UserRouteGroup = () => {
    return (
        <BrowserRouter basename="/">
            <UserNavbar />
            <Routes>
                <Route path="/" element={<Navigate replace to="/home" />}></Route>
                <Route path="sensors" element={<Sensors />}></Route>
                <Route path="home" element={<Home />}></Route>
                <Route path="*" element={<Home />}></Route>
            </Routes>
        </BrowserRouter>
    );
};

export default UserRouteGroup;