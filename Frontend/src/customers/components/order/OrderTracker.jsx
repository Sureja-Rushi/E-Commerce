import { Step, StepLabel, Stepper, StepConnector, stepConnectorClasses   } from '@mui/material'
import React, { useEffect, useState } from 'react'
import { motion } from 'framer-motion'

const steps = [
    "Placed",
    "Order Confirmed",
    "Shipped",
    "Out for Delivery",
    "Delivered",
]

// const OrderTracker = ({activeStep}) => {

//   return (
//     <div className='w-full' >
//         <Stepper activeStep={activeStep} alternativeLabel >
//             {steps.map((label) => <Step>
//                 <StepLabel sx={{color:"#9155fd", fontSize:"40px"}}>{label}</StepLabel>
//             </Step>)}
//         </Stepper>
//     </div>
//   )
// }

const OrderTracker = ({ activeStep }) => {
  const [animatedStep, setAnimatedStep] = useState(0);

  useEffect(() => {
    let step = 0;
    const interval = setInterval(() => {
      if (step <= activeStep) {
        setAnimatedStep(step);
        step++;
      } else {
        clearInterval(interval);
      }
    }, 500); // Adjust animation speed
    return () => clearInterval(interval);
  }, [activeStep]);

  return (
    <div className="w-full">
      <Stepper
        activeStep={animatedStep}
        alternativeLabel
        connector={
          <StepConnector
            sx={{
              [`& .${stepConnectorClasses.line}`]: {
                transition: "all 0.5s ease-in-out",
                borderColor: "#ccc", // Default grey color
                borderWidth: "1px",
              },
              [`&.Mui-active .${stepConnectorClasses.line}, &.Mui-completed .${stepConnectorClasses.line}`]: {
                borderColor: "#007bff", // Purple progress color
                borderWidth: "1px",
              },
            }}
          />
        }
      >
        {steps.map((label, index) => (
          <Step key={index}>
            <motion.div
              // initial={{ scale: 0.8 }}
              // animate={{ scale: animatedStep >= index ? 1.1 : 1 }}
              transition={{ duration: 0.5 }}
            >
              <StepLabel
                sx={{
                  "& .MuiStepLabel-label": {
                    color: animatedStep >= index ? "black" : "gray", // Correctly change step label color
                    fontSize: "16px",
                    fontWeight: animatedStep >= index ? "normal" : "normal",
                  },
                  "& .MuiStepIcon-root": {
                    color: animatedStep >= index ? "#007bff" : "gray", // Change step circle color
                  },
                }}
              >
                {label}
              </StepLabel>
            </motion.div>
          </Step>
        ))}
      </Stepper>
    </div>
  );
};
export default OrderTracker