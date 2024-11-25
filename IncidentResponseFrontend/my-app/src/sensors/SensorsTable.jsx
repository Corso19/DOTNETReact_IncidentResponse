import React, { useEffect, useState } from "react";
import { Col, Row } from "react-bootstrap";
import { CrudService } from "../services/CrudService";
import { ThreeDots } from "react-loader-spinner";
import { Plus } from "react-bootstrap-icons";
import SensorRow from "./SensorRow";

const SensorsTable = () => {

    const [sensors, setSensors] = useState([]);
    const [sensorsLoading, setSensorsLoading] = useState(false);
    const [showAddSensorModal, setShowAddSensorModal] = useState(false);
    useEffect(() => {
        setSensorsLoading(true);
        // get sensors
        CrudService.list("Sensors").then((response) => {
            console.log("Response: ", response);
            if (response.data.success) {
                setSensors(response.data.data);
            }
            setSensorsLoading(false);
        });
    }, []);
    return(
        <Row className="mt-3">
            <Col></Col>
            <Col xs={10}>
                <table data-toggle="table" className="table table-striped table-bordered border-dark">
                    <thead className="text-center">
                        <tr>
                            <th>Sensor</th>
                            <th>Type</th>
                            <th>Enabled</th>
                            <th>
                                <Plus
                                    size={35}
                                    title="Add sensor"
                                    color="green"
                                    className="clickable"
                                    onClick={() => setShowAddSensorModal(true)}
                                />
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    {
                        sensorsLoading ? (
                            <tr>
                                <td colSpan={4}>
                                <div
                                    style={{ width: "100%" }}
                                    className="d-flex justify-content-center"
                                >
                                    <ThreeDots
                                        height="50"
                                        width="50"
                                        color="rgb(200, 200, 200)"
                                        ariaLabel="three-dots-loading"
                                        visible={true}
                                    />
                                    </div>
                                </td>
                            </tr>
                        ) : (
                            sensors.length ? (
                                sensors.map((sensor) => (
                                    <SensorRow
                                        key={sensor.id}
                                        sensor={sensor}
                                        setSensors={setSensors}
                                    />
                                ))
                            ) : (
                                // if there are no sensors, display a message
                                <tr>
                                    <td colSpan={4} className="py-3"><span className="fw-semibold">No sensors added.</span></td>
                                </tr>
                            )
                        )
                    }
                    </tbody>
                </table>
            </Col>
            <Col></Col>
        </Row>
    );
}

export default SensorsTable;