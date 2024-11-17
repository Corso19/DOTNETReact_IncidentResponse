import React from 'react';
import { Spinner, Button} from "react-bootstrap";

const LoaderButton = (props) => {

    return (
        <Button
            disabled={props.conditionForDisable || props.conditionForSpinner ? true : false}
            variant={props.variant}
            className={props.className}
            onClick={props.onClick}
            style={props.style}
            type={props.type}
            size={props.size}
        >
            { 
                props.conditionForSpinner ? (
                    <Spinner
                        as="span"
                        size="sm"
                        role="status"
                        aria-hidden="true"
                        animation="border"
                    />
                ): ( props.text )
            }
                
        </Button>
    );
}


export default LoaderButton;