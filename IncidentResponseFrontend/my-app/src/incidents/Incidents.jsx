import React, { useEffect, useState } from "react";
import { CrudService } from "../services/CrudService";
import { ThreeDots } from "react-bootstrap-icons";
import { Accordion, Col, Row } from "react-bootstrap";
import IncidentItem from "./IncidentItem";

const Incidents = () => {
    const [incidents, setIncidents] = useState([]);
    const [incidentsLoading, setIncidentsLoading] = useState(false);

    useEffect(() => {
        setIncidentsLoading(true);
        // get sensors
        CrudService.list("Incidents").then((response) => {
            if (response.status === 200) {
                setIncidents(response.data);
            }
            setIncidentsLoading(false);
        });
    }, []);

    return(
        <Row className="mt-3">
            <Col sm={12}>
            {
                incidentsLoading ? (
                    <div
                        style={{ width: "100%" }}
                        className="d-flex justify-content-center"
                    >
                        <ThreeDots
                            height="50"
                            width="50"
                            ariaLabel="three-dots-loading"
                            visible={true}
                            color="#005bb5"
                        />
                    </div>
                ) : (
                    incidents.length ? (
                        <Accordion className="mx-3">
                        {
                            incidents.map((incident) => (
                                <IncidentItem
                                    key={incident.incidentId} 
                                    incident={incident} 
                                />
                            ))
                        }
                        </Accordion>
                    ) : (
                        <h5 className="mx-3">No incidents added.</h5>
                    )
                )
            }
            </Col>
        </Row>
    );
}

export default Incidents;