import React, { Children } from "react";
import "./button.scss";

const STYLES = [
    "btn-header",
    "btn-panel"
]
const SIZES = [
    "btn-full-large",
    "btn-full-small"
]
export const Button = ({
    children,
    type,
    onClick,
    buttonStyle,
    buttonSize,

}) => {
    const checkButtonStyles = STYLES.includes(buttonStyle) ? buttonStyle: STYLES[0];
    const checkButtonSize = SIZES.includes(buttonSize) ? buttonSize: SIZES[0];
    
    return (
        <button 
        className={`btn ${checkButtonStyles} 
        ${checkButtonSize}`}
        onClick={onClick} 
        type={type}>
            {children}
        </button>
    )
};