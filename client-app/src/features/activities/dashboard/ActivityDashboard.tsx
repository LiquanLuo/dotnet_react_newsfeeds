import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Grid } from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores/store";
import ActivityFilter from "./ActivityFilter";
import ActivitiyList from "./ActivityList";


export default observer(function ActivitiyDashboard() {
    const { activityStore } = useStore();
    const { loadActivities, activityRegistry } = activityStore;

    useEffect(() => {
        if (activityRegistry.size <= 1) loadActivities();
    }, [loadActivities, activityRegistry]);

    if (activityStore.loadingInitial) return <LoadingComponent content="Loading activities..." />


    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivitiyList />
            </Grid.Column>
            <Grid.Column width='6'>
                <ActivityFilter />
            </Grid.Column>
        </Grid>
    )

})