import React, { useEffect, useState } from "react";
import { Col, Row } from "react-bootstrap";
import { CrudService } from "../services/CrudService";
import { ThreeDots } from "react-loader-spinner";
import { PlusSquareFill } from "react-bootstrap-icons";
import SensorRow from "./SensorRow";

const SensorsTable = () => {

    const [sensors, setSensors] = useState([]);
    const [sensorsLoading, setSensorsLoading] = useState(false);
    const [showAddSensorModal, setShowAddSensorModal] = useState(false);
    useEffect(() => {
        setSensorsLoading(true);
        // get sensors
        // CrudService.list("Sensors").then((response) => {
        //     console.log("Response: ", response);
        //     if (response.data.success) {
        //         setSensors(response.data.data);
        //     }
        //     setSensorsLoading(false);
        // });
        const mock_data = [
            {
              id: 1,
              name: "Temperature Sensor 1",
              type: "Temperature",
              is_enabled: true,
              tenant_id: "tenant_001",
              application_id: "app_xdr_001",
            },
            {
              id: 2,
              name: "Motion Sensor 2",
              type: "Motion",
              is_enabled: false,
              tenant_id: "tenant_002",
              application_id: "app_xdr_002",
            },
            {
              id: 3,
              name: "Vibration Sensor 3",
              type: "Vibration",
              is_enabled: true,
              tenant_id: "tenant_003",
              application_id: "app_xdr_003",
            },
            {
              id: 4,
              name: "Humidity Sensor 4",
              type: "Humidity",
              is_enabled: true,
              tenant_id: "tenant_004",
              application_id: "app_xdr_004",
            },
            {
              id: 5,
              name: "Pressure Sensor 5",
              type: "Pressure",
              is_enabled: false,
              tenant_id: "tenant_005",
              application_id: "app_xdr_005",
            },
            {
                id: 6,
                name: "Temperature Sensor 1",
                type: "Temperature",
                is_enabled: true,
                tenant_id: "tenant_001",
                application_id: "app_xdr_001",
              },
              {
                id: 7,
                name: "Motion Sensor 2",
                type: "Motion",
                is_enabled: false,
                tenant_id: "tenant_002",
                application_id: "app_xdr_002",
              },
              {
                id: 8,
                name: "Vibration Sensor 3",
                type: "Vibration",
                is_enabled: true,
                tenant_id: "tenant_003",
                application_id: "app_xdr_003",
              },
              {
                id: 9,
                name: "Humidity Sensor 4",
                type: "Humidity",
                is_enabled: true,
                tenant_id: "tenant_004",
                application_id: "app_xdr_004",
              },
              {
                id: 10,
                name: "Pressure Sensor 5",
                type: "Pressure",
                is_enabled: false,
                tenant_id: "tenant_005",
                application_id: "app_xdr_005",
              },
              {
                id: 11,
                name: "Temperature Sensor 1",
                type: "Temperature",
                is_enabled: true,
                tenant_id: "tenant_001",
                application_id: "app_xdr_001",
              },
              {
                id: 12,
                name: "Motion Sensor 2",
                type: "Motion",
                is_enabled: false,
                tenant_id: "tenant_002",
                application_id: "app_xdr_002",
              },
              {
                id: 13,
                name: "Vibration Sensor 3",
                type: "Vibration",
                is_enabled: true,
                tenant_id: "tenant_003",
                application_id: "app_xdr_003",
              },
              {
                id: 14,
                name: "Humidity Sensor 4",
                type: "Humidity",
                is_enabled: true,
                tenant_id: "tenant_004",
                application_id: "app_xdr_004",
              },
              {
                id: 15,
                name: "Pressure Sensor 5",
                type: "Pressure",
                is_enabled: false,
                tenant_id: "tenant_005",
                application_id: "app_xdr_005",
              },
              {
                id: 16,
                name: "Temperature Sensor 1",
                type: "Temperature",
                is_enabled: true,
                tenant_id: "tenant_001",
                application_id: "app_xdr_001",
              },
              {
                id: 17,
                name: "Motion Sensor 2",
                type: "Motion",
                is_enabled: false,
                tenant_id: "tenant_002",
                application_id: "app_xdr_002",
              },
              {
                id: 18,
                name: "Vibration Sensor 3",
                type: "Vibration",
                is_enabled: true,
                tenant_id: "tenant_003",
                application_id: "app_xdr_003",
              },
              {
                id: 19,
                name: "Humidity Sensor 4",
                type: "Humidity",
                is_enabled: true,
                tenant_id: "tenant_004",
                application_id: "app_xdr_004",
              },
              {
                id: 20,
                name: "Pressure Sensor 5",
                type: "Pressure",
                is_enabled: false,
                tenant_id: "tenant_005",
                application_id: "app_xdr_005",
              },
              {
                id: 21,
                name: "Temperature Sensor 1",
                type: "Temperature",
                is_enabled: true,
                tenant_id: "tenant_001",
                application_id: "app_xdr_001",
              },
              {
                id: 22,
                name: "Motion Sensor 2",
                type: "Motion",
                is_enabled: false,
                tenant_id: "tenant_002",
                application_id: "app_xdr_002",
              },
              {
                id: 23,
                name: "Vibration Sensor 3",
                type: "Vibration",
                is_enabled: true,
                tenant_id: "tenant_003",
                application_id: "app_xdr_003",
              },
              {
                id: 24,
                name: "Humidity Sensor 4",
                type: "Humidity",
                is_enabled: true,
                tenant_id: "tenant_004",
                application_id: "app_xdr_004",
              },
              {
                id: 25,
                name: "Pressure Sensor 5",
                type: "Pressure",
                is_enabled: false,
                tenant_id: "tenant_005",
                application_id: "app_xdr_005",
              }
            ];
        setSensors(mock_data);
        setSensorsLoading(false);
    }, []);
    return(
        <Row className="mt-3">
            <Col >
                <table data-toggle="table" className="table table-bordered table-striped sensors-table">
                    <thead className="text-center">
                        <tr>
                            <th style={{ width: "35%" }}>Sensor</th>
                            <th style={{ width: "35%" }}>Type</th>
                            <th style={{ width: "20%" }}>Enabled</th>
                            <th>
                                <PlusSquareFill
                                    size={22}
                                    title="Add sensor"
                                    className="clickable color-bg-lightr"
                                    cursor="pointer"
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
                                        className="color-primary-dark"
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
        </Row>
    );
}

export default SensorsTable;