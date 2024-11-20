import React from 'react';
import { Navbar, Nav, Container } from 'react-bootstrap';

const UserNavbar = () => {
    return (
        <Navbar expand="sm" className="xdr-navbar py-4">
            <Container>
                <Navbar.Brand href="#">My XDR App</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="col d-flex justify-content-start">
                        <Nav.Link eventKey={1} href="/home" className="mx-3">Home</Nav.Link>
                        <Nav.Link eventKey={2} href="/sensors" id="basic-nav-dropdown" className="mx-3">Sensors</Nav.Link>
                    </Nav> 
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default UserNavbar;