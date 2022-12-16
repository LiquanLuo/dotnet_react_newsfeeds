import React, { useEffect, useState } from "react";
import axios from "axios";
import { Container } from "semantic-ui-react";
import { Activity } from "../model/activity";
import NavBar from "./NavBar";
import ActivitiyDashboard from "../../features/activities/dashboard/ActivityDashboard";
import { v4 as uuid } from "uuid"

function App() {
  const [activities, setActivities] = useState<Activity[]>([]);
  const [seletctedActivity, setSeletctedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);
  useEffect(() => {
    axios
      .get<Activity[]>("http://localhost:5000/api/Activities")
      .then((response) => {
        setActivities(response.data);
      });
  }, []);

  function handleSelectActivity(id: string) {
    setSeletctedActivity(activities.find(x => x.id === id));
  }

  function handleCancelSelectActivity() {
    setSeletctedActivity(undefined);
  }

  function handleFormOn(id?: string) {
    id ? handleSelectActivity(id) : handleCancelSelectActivity();
    setEditMode(true);
  }

  function handleFormClose() {
    setEditMode(false);
  }

  function handleEditCreateActivity(activity: Activity) {
    activity.id
      ? setActivities([...activities.filter(x => x.id !== activity.id), activity])
      : setActivities([...activities, { ...activity, id: uuid()}]);
    setEditMode(false);
    setSeletctedActivity(activity);
  }

  function handleDeleteActivity(id : string) {
    setActivities([...activities.filter(x => x.id !== id)]);
    handleCancelSelectActivity();
  }

  return (
    <>
      <NavBar openForm={handleFormOn} />
      <Container style={{ marginTop: '7em' }}>
        <ActivitiyDashboard activities={activities}
          selectedActivity={seletctedActivity}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectActivity}
          editMode={editMode}
          openForm={handleFormOn}
          closeForm={handleFormClose}
          handleEditCreateActivity={handleEditCreateActivity}
          deleteActivity={handleDeleteActivity}
        />
      </Container>

    </>
  );
}

export default App;
