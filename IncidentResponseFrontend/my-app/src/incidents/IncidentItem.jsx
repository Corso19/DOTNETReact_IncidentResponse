import React from "react";
import { Accordion } from "react-bootstrap";

const IncidentItem = ({incident}) => {

    return(
        <Accordion.Item eventKey={incident.incidentId}>
            <Accordion.Header>{incident.title}</Accordion.Header>
            <Accordion.Body></Accordion.Body>
        </Accordion.Item>
    );
}

export default IncidentItem;