interface GradeViewProps {
    currentValue: number;
    maxValue: number;
}

function GradeView({ currentValue, maxValue }: GradeViewProps) {

    const gradeClassName = defineGradeClassName(currentValue);

    function defineGradeClassName(value: number) {
        if (value < 3) return 'grade-red';

        if (value === 3) return 'grade-yellow';

        if (value === 5) return 'grade-green';

        return '';
    }

    return (
        <div className={`grade ${gradeClassName}`}>
            <span>{currentValue}</span>&nbsp;
            <span>/&nbsp;{maxValue}</span>
        </div>
    );
}

export default GradeView;