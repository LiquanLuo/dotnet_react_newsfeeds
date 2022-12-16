import React from "react";
import { Grid } from "semantic-ui-react";
import { Activity } from "../../../app/model/activity";
import ActivityDetails from "../details/ActivityDetails";
import ActivitiyForm from "../form/ActivityForm";
import ActivitiyList from "./ActivityList";


interface Props {
    activities: Activity[];
    selectedActivity: Activity | undefined;
    selectActivity: (id: string) => void;
    cancelSelectActivity: () => void;
    editMode: boolean;
    openForm: (id: string) => void;
    closeForm: () => void;
    handleEditCreateActivity: (activity: Activity) => void;
    deleteActivity: (id: string) => void;
}

export default function ActivitiyDashboard({ activities,
    selectedActivity,
    selectActivity,
    cancelSelectActivity,
    editMode,
    openForm,
    closeForm,
    handleEditCreateActivity,
    deleteActivity }: Props) {
    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivitiyList activities={activities} selectActivity={selectActivity} deleteActivity={deleteActivity} />
            </Grid.Column>
            <Grid.Column width='6'>
                {selectedActivity && !editMode &&
                    <ActivityDetails
                        activity={selectedActivity}
                        cancelSelectActivity={cancelSelectActivity}
                        openForm={openForm}
                    />}
                {editMode &&
                    <ActivitiyForm
                        closeForm={closeForm}
                        selectedActivity={selectedActivity}
                        handleEditCreateActivity={handleEditCreateActivity} />}
            </Grid.Column>
        </Grid>
    )

}