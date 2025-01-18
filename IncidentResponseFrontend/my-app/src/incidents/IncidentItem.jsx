import React from "react";
import { Accordion } from "react-bootstrap";

const IncidentItem = ({incident}) => {
    return(
        <Accordion.Item eventKey={incident.incidentId} className="mb-1">
            <Accordion.Header className="accordion-header">{incident.title}</Accordion.Header>
            <Accordion.Body className="accordion-body mb-1"></Accordion.Body>
        </Accordion.Item>
    );
}

export default IncidentItem;