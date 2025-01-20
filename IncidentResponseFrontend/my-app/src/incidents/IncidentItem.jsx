import React from "react";
import { Accordion, Tab, Tabs } from "react-bootstrap";

const IncidentItem = ({incident}) => {
    return(
        <Accordion.Item eventKey={incident.incidentId} className="mb-1">
            <Accordion.Header className="accordion-header">{incident.title} - {incident.detectedAt}</Accordion.Header>
            <Accordion.Body className="accordion-body">
                <Tabs
                    defaultActiveKey="incident"
                    id={`incident-id-${incident.incidentId}`}
                    className="mt-2 mb-4 incident-tabs"
                    fill
                >
                    <Tab eventKey="entity" title="Entity">
                        Entity
                    </Tab>
                    <Tab eventKey="resource" title="Resource">
                        Resource
                    </Tab>
                    <Tab eventKey="recomandation" title="Recomandation">
                        Recomandation
                    </Tab>
                </Tabs>
            </Accordion.Body>
        </Accordion.Item>
    );
}

export default IncidentItem;