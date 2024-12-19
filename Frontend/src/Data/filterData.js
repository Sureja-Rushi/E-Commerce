export const color = [
    "white",
    "Black",
    "Red",
    "Maroon",
    "Being",
    "Pink",
    "Green",
    "Yellow",
];

export const filters = [
    {
        id: "color",
        name:"Color",
        options: [
            {value:"white", label:"White"},
            {value:"black", label:"Black"},
            {value:"red", label:"Red"},
            {value:"blue", label:"Blue"},
            {value:"maroon", label:"Maroon"},
            {value:"being", label:"Being"},
            {value:"pink", label:"Pink"},
            {value:"green", label:"Green"},
            {value: "purple", label: "Purple"},
            {value:"yellow", label:"Yellow"},
        ],
    },

    {
        id: "size",
        name: "Size",
        options: [
            {value: "s", label: "S"},
            {value: "m", label: "M"},
            {value: "l", label: "L"},
            {value: "xl", label: "XL"},
            {value: "xxl", label: "XXL"},
        ],
    },
];

export const singleFilter = [
    {
        id:"price",
        name: "Price",
        options:[
            {value: "0-499", label: "₹0 To ₹499"},
            {value: "499-799", label: "₹499 To ₹799"},
            {value: "799-999", label: "₹799 To ₹999"},
            {value: "999-1999", label: "₹999 To ₹1999"},
            {value: "1999-2499", label: "₹1999 To ₹2499"},
            {value: "2499+", label:"₹2499+"}
        ],
    },
];