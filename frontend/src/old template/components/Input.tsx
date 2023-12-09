import { useState } from "react";

interface InputProps {
    required: boolean;
}

export function Input(props: InputProps) {
    const [value, setValue] = useState('');
    const [error, setError] = useState('');

}